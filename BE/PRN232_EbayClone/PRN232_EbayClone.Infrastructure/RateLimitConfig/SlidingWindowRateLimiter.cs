using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
    private readonly RequestDelegate _next;
    private readonly bool _redisAvailable;

    public SlidingWindowRateLimiter(RequestDelegate next, IConfiguration config, IServiceProvider serviceProvider, IOptionsMonitor<List<RateLimitRule>> rulesMonitor)
    {
        _next = next;
        _config = config;
        _rulesMonitor = rulesMonitor;
        try
        {
            var muxer = serviceProvider.GetService<IConnectionMultiplexer>();
            _db = muxer?.GetDatabase();
            _redisAvailable = _db != null;
        }
        catch
        {
            _redisAvailable = false;
        }
    }

    private static string GetUserKey(HttpContext context)
    {
        var userId = context.User?.FindFirst("sub")?.Value
                     ?? context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userId))
            return userId;

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
        return (int)await _db.ScriptEvaluateAsync(SlidingRateLimiter, keys, args.ToArray()) == 1;
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
