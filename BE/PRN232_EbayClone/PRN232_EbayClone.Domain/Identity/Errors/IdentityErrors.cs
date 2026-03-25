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
}
