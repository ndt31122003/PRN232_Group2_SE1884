using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Policies.Errors;

public static class PolicyErrors
{
    public static readonly Error NameRequired = Error.Validation(
        "Policy.NameRequired",
        "Tên chính sách là bắt buộc");

    public static readonly Error ReturnPeriodRequired = Error.Validation(
        "Policy.ReturnPeriodRequired",
        "Thời gian trả hàng là bắt buộc khi chấp nhận trả hàng");

    public static readonly Error RefundMethodRequired = Error.Validation(
        "Policy.RefundMethodRequired",
        "Phương thức hoàn tiền là bắt buộc khi chấp nhận trả hàng");

    public static readonly Error ReturnShippingPaidByRequired = Error.Validation(
        "Policy.ReturnShippingPaidByRequired",
        "Người trả phí vận chuyển trả hàng là bắt buộc khi chấp nhận trả hàng");

    public static readonly Error InvalidHandlingTime = Error.Validation(
        "Policy.InvalidHandlingTime",
        "Thời gian xử lý phải từ 0 đến 30 ngày");

    public static readonly Error NotFound = Error.Failure(
        "Policy.NotFound",
        "Chính sách không tồn tại");

    public static readonly Error Unauthorized = Error.Failure(
        "Policy.Unauthorized",
        "Bạn không có quyền thực hiện thao tác này");

    public static readonly Error AlreadyExists = Error.Failure(
        "Policy.AlreadyExists",
        "Chính sách đã tồn tại cho cửa hàng này");
}

