using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using PRN232_EbayClone.Api.Infrastructure.RateLimitConfig;
using StackExchange.Redis;

namespace PRN232_EbayClone.Infrastructure.RateLimitConfig;

public class SlidingWindowRateLimiter
{
    private const string SlidingRateLimiter = @"
            local current_time = redis.call('TIME')
            local num_windows = ARGV[1]
            for i=2, num_windows*2, 2 do
                local window = ARGV[i]
                local max_requests = ARGV[i+1]
                local curr_key = KEYS[i/2]
                local trim_time = tonumber(current_time[1]) - window
                redis.call('ZREMRANGEBYSCORE', curr_key, 0, trim_time)
                local request_count = redis.call('ZCARD',curr_key)
                if request_count >= tonumber(max_requests) then
                    return 1
                end
            end
            for i=2, num_windows*2, 2 do
                local curr_key = KEYS[i/2]
                local window = ARGV[i]
                redis.call('ZADD', curr_key, current_time[1], current_time[1] .. current_time[2])
                redis.call('EXPIRE', curr_key, window)
            end
            return 0
            ";
    private readonly IDatabase? _db;
    private readonly IConfiguration _config;
    private readonly IOptionsMonitor<List<RateLimitRule>> _rulesMonitor;
    private readonly ILogger<SlidingWindowRateLimiter> _logger;
    private readonly RequestDelegate _next;
    private readonly bool _redisAvailable;

    public SlidingWindowRateLimiter(RequestDelegate next, IConfiguration config, IServiceProvider serviceProvider, IOptionsMonitor<List<RateLimitRule>> rulesMonitor)
    {
        _next = next;
        _config = config;
        _rulesMonitor = rulesMonitor;
        _logger = serviceProvider.GetRequiredService<ILogger<SlidingWindowRateLimiter>>();
        try
        {
            var muxer = serviceProvider.GetService<IConnectionMultiplexer>();
            _db = muxer?.GetDatabase();
            _redisAvailable = _db != null;
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Redis rate limiter is unavailable during middleware initialization. Falling back to pass-through mode.");
            _redisAvailable = false;
        }
    }

    private static string GetUserKey(HttpContext context)
    {
        var userId = context.User?.FindFirst("sub")?.Value
                     ?? context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userId))
            return userId;

        // Cloudflare Tunnel injects the real client IP in CF-Connecting-IP
        var cfIp = context.Request.Headers["CF-Connecting-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(cfIp))
            return cfIp;

        // Generic reverse-proxy fallback
        var forwarded = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwarded))
            return forwarded.Split(',')[0].Trim();

        return context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
    }


    public IEnumerable<RateLimitRule> GetApplicableRules(HttpContext context)
    {
        var limits = _rulesMonitor.CurrentValue;
        return limits
            .Where(x => x.MatchPath(context.Request.Path))
            .OrderBy(x => x.MaxRequests)
            .GroupBy(x => new { x.PathKey, x.WindowSeconds })
            .Select(x => x.First());
    }

    private async Task<bool> IsLimited(IEnumerable<RateLimitRule> rules, string apiKey)
    {
        var keys = rules.Select(x => new RedisKey($"{x.PathKey}:{{{apiKey}}}:{x.WindowSeconds}")).ToArray();
        var args = new List<RedisValue> { rules.Count() };
        foreach (var rule in rules)
        {
            args.Add(rule.WindowSeconds);
            args.Add(rule.MaxRequests);
        }

        // Use a short timeout so a slow/unavailable Redis Cloud never blocks the HTTP pipeline
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
        try
        {
            var task = _db!.ScriptEvaluateAsync(SlidingRateLimiter, keys, args.ToArray());

            // Await with timeout; if Redis takes > 2 s, fail-open (allow request)
            var completed = await Task.WhenAny(task, Task.Delay(Timeout.Infinite, cts.Token));
            if (completed != task)
            {
                _logger.LogWarning("Redis rate limiting timed out for key {ApiKey}. Allowing request to continue.", apiKey);
                return false;
            }

            return (int)await task == 1;
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Redis rate limiting failed for key {ApiKey}. Allowing request to continue.", apiKey);
            return false;
        }
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (!_redisAvailable)
        {
            await _next(httpContext);
            return;
        }

        var apiKey = GetUserKey(httpContext);

        if (string.IsNullOrEmpty(apiKey))
        {
            httpContext.Response.StatusCode = 401;
            return;
        }

        var applicableRules = GetApplicableRules(httpContext);
        if (!applicableRules.Any())
        {
            await _next(httpContext);
            return;
        }

        if (await IsLimited(applicableRules, apiKey))
        {
            httpContext.Response.StatusCode = 429;
            httpContext.Response.ContentType = "application/json";

            var maxWindow = applicableRules.Max(x => x.WindowSeconds);
            httpContext.Response.Headers.Append("Retry-After", maxWindow.ToString());

            await httpContext.Response.WriteAsync("Too many requests. Please try again later.");
            return;
        }

        await _next(httpContext);
    }
}
