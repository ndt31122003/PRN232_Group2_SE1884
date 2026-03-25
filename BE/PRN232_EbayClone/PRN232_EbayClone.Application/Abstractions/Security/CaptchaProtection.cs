namespace PRN232_EbayClone.Application.Abstractions.Security;

public static class CaptchaActions
{
    public const string IdentityLogin = "identity.login";
    public const string IdentityRegister = "identity.register";
    public const string IdentityForgotPassword = "identity.forgot-password";
    public const string IdentityResetPassword = "identity.reset-password";
    public const string IdentityResendOtp = "identity.resend-otp";
}

public sealed record CaptchaPolicyDecision(
    string ActionName,
    CaptchaRequirementType RequirementType,
    int FailedAttempts,
    int Threshold)
{
    public bool IsCaptchaRequired => RequirementType == CaptchaRequirementType.Always
        || (RequirementType == CaptchaRequirementType.Conditional && FailedAttempts >= Threshold);
}

public enum CaptchaRequirementType
{
    None = 0,
    Always = 1,
    Conditional = 2
}

public interface IRecaptchaVerificationService
{
    Task<RecaptchaVerificationResult> VerifyAsync(string token, CancellationToken cancellationToken);
}

public sealed record RecaptchaVerificationResult(bool IsSuccess, string? Action);

public interface ICaptchaProtectionService
{
    Task<Result> EnsureValidAsync(
        string actionName,
        string? captchaToken,
        string? captchaAction,
        string? identityHint,
        CancellationToken cancellationToken);

    Task RegisterFailureAsync(string actionName, string? identityHint, CancellationToken cancellationToken);

    Task RegisterSuccessAsync(string actionName, string? identityHint, CancellationToken cancellationToken);

    Task<CaptchaPolicyDecision> GetDecisionAsync(string actionName, string? identityHint, CancellationToken cancellationToken);
}
