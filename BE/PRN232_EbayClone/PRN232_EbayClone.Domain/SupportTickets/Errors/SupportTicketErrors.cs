using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.SupportTickets.Errors;

public static class SupportTicketErrors
{
    public static readonly Error NotFound = Error.Failure(
        "SupportTicket.NotFound",
        "Ticket hỗ trợ không tồn tại");

    public static readonly Error Unauthorized = Error.Failure(
        "SupportTicket.Unauthorized",
        "Bạn không có quyền thực hiện hành động này");

    public static readonly Error Closed = Error.Failure(
        "SupportTicket.Closed",
        "Không thể thêm phản hồi vào ticket đã đóng");

    public static readonly Error InvalidCategory = Error.Validation(
        "SupportTicket.InvalidCategory",
        "Danh mục không hợp lệ");

    public static readonly Error NumberGenerationFailed = Error.Failure(
        "SupportTicket.NumberGenerationFailed",
        "Không thể tạo số ticket. Vui lòng thử lại");
}
