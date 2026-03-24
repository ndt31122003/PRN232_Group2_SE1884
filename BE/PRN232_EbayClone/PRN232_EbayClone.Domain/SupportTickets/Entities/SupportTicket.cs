using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.SupportTickets.Enums;
using PRN232_EbayClone.Domain.SupportTickets.Errors;

namespace PRN232_EbayClone.Domain.SupportTickets.Entities;

public class SupportTicket : AggregateRoot<Guid>
{
    public string SellerId { get; private set; } = null!;
    public string Category { get; private set; } = null!;
    public string Subject { get; private set; } = null!;
    public string Message { get; private set; } = null!;
    public string Status { get; private set; } = null!;
    public bool IsDeleted { get; private set; }

    private SupportTicket(Guid id) : base(id) { }

    public static Result<SupportTicket> Create(
        string sellerId,
        string category,
        string subject,
        string message)
    {
        if (string.IsNullOrWhiteSpace(sellerId))
        {
            return Error.Validation("SupportTicket.SellerIdRequired", "Seller ID là bắt buộc");
        }

        if (string.IsNullOrWhiteSpace(subject))
        {
            return SupportTicketErrors.SubjectRequired;
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            return SupportTicketErrors.MessageRequired;
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            return Error.Validation("SupportTicket.CategoryRequired", "Danh mục là bắt buộc");
        }

        var ticket = new SupportTicket(Guid.NewGuid())
        {
            SellerId = sellerId,
            Category = category,
            Subject = subject,
            Message = message,
            Status = TicketStatus.Open.ToString(),
            IsDeleted = false
        };

        return Result.Success(ticket);
    }

    public Result UpdateStatus(string newStatus)
    {
        if (!Enum.TryParse<TicketStatus>(newStatus, ignoreCase: true, out var status))
        {
            return Error.Validation("SupportTicket.InvalidStatus", $"Trạng thái không hợp lệ: {newStatus}");
        }

        // Validate state transitions
        var currentStatus = Enum.Parse<TicketStatus>(Status, ignoreCase: true);
        
        var isValidTransition = (currentStatus, status) switch
        {
            (TicketStatus.Open, TicketStatus.Pending) => true,
            (TicketStatus.Open, TicketStatus.Resolved) => true,
            (TicketStatus.Pending, TicketStatus.Resolved) => true,
            (TicketStatus.Resolved, TicketStatus.Closed) => true,
            (TicketStatus.Resolved, TicketStatus.Open) => true, // Reopen
            _ when currentStatus == status => true, // Same status
            _ => false
        };

        if (!isValidTransition)
        {
            return SupportTicketErrors.InvalidTransition;
        }

        if (IsDeleted)
        {
            return SupportTicketErrors.CannotUpdate;
        }

        Status = newStatus;
        return Result.Success();
    }

    public Result Delete()
    {
        if (Status == TicketStatus.Closed.ToString())
        {
            return Error.Validation("SupportTicket.CannotDeleteClosed", "Không thể xóa ticket đã đóng");
        }

        IsDeleted = true;
        return Result.Success();
    }

    public bool IsClosed => Status == TicketStatus.Closed.ToString();
    public bool IsOpen => Status == TicketStatus.Open.ToString();
}