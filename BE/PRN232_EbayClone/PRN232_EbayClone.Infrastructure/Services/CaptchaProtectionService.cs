using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using PRN232_EbayClone.Application.Abstractions.Security;
using PRN232_EbayClone.Domain.Identity.Errors;
using PRN232_EbayClone.Domain.Shared.Results;
using System.Security.Claims;

namespace PRN232_EbayClone.Infrastructure.Services;

public sealed class CaptchaProtectionService : ICaptchaProtectionService
{
    private static readonly IReadOnlyDictionary<string, (CaptchaRequirementType Type, int Threshold)> Rules =
        new Dictionary<string, (CaptchaRequirementType Type, int Threshold)>(StringComparer.OrdinalIgnoreCase)
        {
            [CaptchaActions.IdentityLogin] = (CaptchaRequirementType.Conditional, 3),
            [CaptchaActions.IdentityRegister] = (CaptchaRequirementType.Always, 0),
            [CaptchaActions.IdentityForgotPassword] = (CaptchaRequirementType.Always, 0),
            [CaptchaActions.IdentityResetPassword] = (CaptchaRequirementType.Always, 0),
            [CaptchaActions.IdentityResendOtp] = (CaptchaRequirementType.None, 0)
        };

    private static readonly TimeSpan FailedAttemptTtl = TimeSpan.FromMinutes(30);

    private readonly IDistributedCache _distributedCache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRecaptchaVerificationService _recaptchaVerificationService;
    private readonly ILogger<CaptchaProtectionService> _logger;

    public CaptchaProtectionService(
        IDistributedCache distributedCache,
        IHttpContextAccessor httpContextAccessor,
        IRecaptchaVerificationService recaptchaVerificationService,
        ILogger<CaptchaProtectionService> logger)
    {
        _distributedCache = distributedCache;
        _httpContextAccessor = httpContextAccessor;
        _recaptchaVerificationService = recaptchaVerificationService;
        _logger = logger;
    }

    public async Task<Result> EnsureValidAsync(
        string actionName,
        string? captchaToken,
        string? captchaAction,
        string? identityHint,
        CancellationToken cancellationToken)
    {
        var decision = await GetDecisionAsync(actionName, identityHint, cancellationToken);
        if (!decision.IsCaptchaRequired)
        {
            return Result.Success();
        }

        if (string.IsNullOrWhiteSpace(captchaToken) || string.IsNullOrWhiteSpace(captchaAction))
        {
            return IdentityErrors.CaptchaRequired;
        }

        if (!string.Equals(captchaAction, actionName, StringComparison.OrdinalIgnoreCase))
        {
            return IdentityErrors.CaptchaActionMismatch;
        }

        var verificationResult = await _recaptchaVerificationService.VerifyAsync(captchaToken, cancellationToken);
        if (!verificationResult.IsSuccess)
        {
            return IdentityErrors.InvalidCaptcha;
        }

        if (!string.IsNullOrWhiteSpace(verificationResult.Action)
            && !string.Equals(verificationResult.Action, actionName, StringComparison.OrdinalIgnoreCase))
        {
            return IdentityErrors.CaptchaActionMismatch;
        }

        return Result.Success();
    }

    public async Task RegisterFailureAsync(string actionName, string? identityHint, CancellationToken cancellationToken)
    {
        var decision = await GetDecisionAsync(actionName, identityHint, cancellationToken);
        if (decision.RequirementType != CaptchaRequirementType.Conditional)
        {
            return;
        }

        var cacheKey = BuildCacheKey(actionName, identityHint);
        var failedAttempts = decision.FailedAttempts + 1;
        var data = BitConverter.GetBytes(failedAttempts);

        try
        {
            await _distributedCache.SetAsync(cacheKey, data, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = FailedAttemptTtl
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis unavailable when recording captcha failure for {Action}. Skipping.", actionName);
        }
    }

    public async Task RegisterSuccessAsync(string actionName, string? identityHint, CancellationToken cancellationToken)
    {
        var decision = await GetDecisionAsync(actionName, identityHint, cancellationToken);
        if (decision.RequirementType != CaptchaRequirementType.Conditional)
        {
            return;
        }

        var cacheKey = BuildCacheKey(actionName, identityHint);
        try
        {
            await _distributedCache.RemoveAsync(cacheKey, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis unavailable when clearing captcha state for {Action}. Skipping.", actionName);
        }
    }

    public async Task<CaptchaPolicyDecision> GetDecisionAsync(string actionName, string? identityHint, CancellationToken cancellationToken)
    {
        if (!Rules.TryGetValue(actionName, out var rule))
        {
            return new CaptchaPolicyDecision(actionName, CaptchaRequirementType.None, 0, 0);
        }

        if (rule.Type != CaptchaRequirementType.Conditional)
        {
            return new CaptchaPolicyDecision(actionName, rule.Type, 0, rule.Threshold);
        }

        var cacheKey = BuildCacheKey(actionName, identityHint);
        byte[]? payload = null;
        try
        {
            payload = await _distributedCache.GetAsync(cacheKey, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis unavailable when reading captcha state for {Action}. Defaulting to 0 failed attempts.", actionName);
        }

        var failedAttempts = payload is { Length: 4 }
            ? BitConverter.ToInt32(payload)
            : 0;

        return new CaptchaPolicyDecision(actionName, rule.Type, failedAttempts, rule.Threshold);
    }

    private string BuildCacheKey(string actionName, string? identityHint)
    {
        var principal = _httpContextAccessor.HttpContext?.User;
        var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? principal?.FindFirstValue("sub");

        var source = !string.IsNullOrWhiteSpace(userId)
            ? $"user:{userId}"
            : !string.IsNullOrWhiteSpace(identityHint)
                ? $"hint:{identityHint.Trim().ToLowerInvariant()}"
                : $"ip:{_httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown"}";

        return $"captcha:failed:{actionName}:{source}";
    }
}
