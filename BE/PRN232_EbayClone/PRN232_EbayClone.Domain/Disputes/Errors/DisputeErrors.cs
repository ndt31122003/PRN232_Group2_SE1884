using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Disputes.Errors;

public static class DisputeErrors
{
    public static readonly Error NotFound = Error.Failure(
        "Dispute.NotFound",
        "Khiếu nại không tồn tại");

    public static readonly Error Unauthorized = Error.Failure(
        "Dispute.Unauthorized",
        "Bạn không có quyền thực hiện hành động này");

    public static readonly Error AlreadyExists = Error.Failure(
        "Dispute.AlreadyExists",
        "Đã tồn tại khiếu nại cho listing này");

    public static readonly Error CannotUpdate = Error.Failure(
        "Dispute.CannotUpdate",
        "Không thể cập nhật khiếu nại ở trạng thái hiện tại");

    public static readonly Error ReasonRequired = Error.Validation(
        "Dispute.ReasonRequired",
        "Lý do khiếu nại là bắt buộc");

    public static readonly Error ListingNotFound = Error.Failure(
        "Dispute.ListingNotFound",
        "Listing không tồn tại");
}
