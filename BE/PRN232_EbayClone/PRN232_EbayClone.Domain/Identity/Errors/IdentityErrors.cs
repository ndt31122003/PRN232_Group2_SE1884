using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Identity.Errors;

public static class IdentityErrors
{
    public static readonly Error InvalidCredentials = Error.Failure(
        "Identity.InvalidCredentials",
        "Tên tài khoản hoặc mật khẩu không đúng");

    public static readonly Error InvalidRefreshToken = Error.Failure(
        "Unauthorized",
        "Refresh token không hợp lệ");

    public static readonly Error EmailNotVerified = Error.Failure(
        "Identity.EmailNotVerified",
        "Email chưa được xác thực");

    public static readonly Error InvalidOtp = Error.Failure(
        "Identity.OtpNotFound",
        "Mã xác thực không hợp lệ hoặc đã hết hạn");

    public static readonly Error OtpAlreadySent = Error.Failure(
        "Identity.OtpAlreadySent",
        "Mã xác thực đã được gửi, vui lòng kiểm tra email của bạn");

    public static readonly Error CaptchaRequired = Error.Validation(
        "Identity.CaptchaRequired",
        "Vui lòng xác thực CAPTCHA trước khi tiếp tục");

    public static readonly Error InvalidCaptcha = Error.Validation(
        "Identity.InvalidCaptcha",
        "CAPTCHA không hợp lệ hoặc đã hết hạn");

    public static readonly Error CaptchaActionMismatch = Error.Validation(
        "Identity.CaptchaActionMismatch",
        "Ngữ cảnh CAPTCHA không hợp lệ cho thao tác hiện tại");
}
