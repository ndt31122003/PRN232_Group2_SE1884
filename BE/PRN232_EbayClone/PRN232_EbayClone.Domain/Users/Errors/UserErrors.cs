using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Domain.Users.Errors;
public class UserErrors
{
    public static Error DuplicateEmail(Email email) => Error.Failure(
        "User.DuplicateEmail",
        $"Email {email.Value} đã tồn tại trong hệ thống");

    public static readonly Error NoRolesAssigned = Error.Failure(
        "User.NoRolesAssigned",
        "Người dùng phải có ít nhất 1 vai trò");

    public static readonly Error NotFound = Error.Failure(
        "User.NotFound",
        "Người dùng không tồn tại");

    public static readonly Error EmailAlreadyVerified = Error.Failure(
        "User.EmailAlreadyVerified",
        "Email đã được xác thực");

    public static readonly Error Unauthorized = Error.Failure(
        "Unauthorized",
        "Bạn không có quyền thực hiện hành động này.");

    public static readonly Error SellerNotVerified = Error.Failure(
        "User.SellerNotVerified",
        "Tài khoản bán hàng của bạn cần được xác minh trước khi tạo tin đăng.");

    public static readonly Error EmailNotVerified = Error.Failure(
        "User.EmailNotVerified",
        "Bạn cần xác minh email trước.");

    public static readonly Error PaymentNotVerified = Error.Failure(
        "User.PaymentNotVerified",
        "Bạn cần xác minh phương thức thanh toán trước.");

    public static readonly Error PaymentAlreadyVerified = Error.Failure(
        "User.PaymentAlreadyVerified",
        "Phương thức thanh toán đã được xác minh.");

    public static readonly Error InvalidPaymentCard = Error.Failure(
        "User.InvalidPaymentCard",
        "Thông tin thẻ thanh toán không hợp lệ hoặc đã hết hạn.");

    public static readonly Error PhoneAlreadyVerified = Error.Failure(
        "User.PhoneAlreadyVerified",
        "Số điện thoại đã được xác minh.");

    public static readonly Error PhoneNotVerified = Error.Failure(
        "User.PhoneNotVerified",
        "Bạn cần xác minh số điện thoại trước.");

    public static readonly Error InvalidPhoneNumber = Error.Failure(
        "User.InvalidPhoneNumber",
        "Số điện thoại không hợp lệ.");

    public static readonly Error BusinessAlreadyVerified = Error.Failure(
        "User.BusinessAlreadyVerified",
        "Thông tin doanh nghiệp đã được xác minh.");
}
