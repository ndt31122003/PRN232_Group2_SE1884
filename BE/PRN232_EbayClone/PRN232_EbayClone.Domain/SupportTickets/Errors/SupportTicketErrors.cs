using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.SupportTickets.Errors;

public static class SupportTicketErrors
{
    public static readonly Error SubjectRequired = Error.Validation(
        "SupportTicket.SubjectRequired", 
        "Tiêu đề ticket là bắt buộc");

    public static readonly Error MessageRequired = Error.Validation(
        "SupportTicket.MessageRequired", 
        "Nội dung ticket là bắt buộc");

    public static readonly Error CannotUpdate = Error.Validation(
        "SupportTicket.CannotUpdate", 
        "Không thể cập nhật ticket đã đóng");

    public static readonly Error NotFound = Error.NotFound(
        "SupportTicket.NotFound", 
        "Không tìm thấy support ticket");

    public static readonly Error InvalidTransition = Error.Validation(
        "SupportTicket.InvalidTransition", 
        "Chuyển đổi trạng thái không hợp lệ");
}