using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Disputes.Entities;

public enum SenderRole
{
    Buyer,
    Seller,
    Admin
}

public sealed class DisputeMessage : Entity<Guid>
{
    public const int MaxMessageLength = 2000;

    public Guid DisputeId { get; private set; }
    public Guid SenderId { get; private set; }
    public SenderRole SenderRole { get; private set; }
    public string MessageText { get; private set; } = string.Empty;
    public DateTimeOffset SentAt { get; private set; }

    // Audit fields
    public DateTimeOffset CreatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation properties
    public Dispute? Dispute { get; private set; }

    private DisputeMessage() : base(Guid.Empty)
    {
    }

    private DisputeMessage(
        Guid id,
        Guid disputeId,
        Guid senderId,
        SenderRole senderRole,
        string messageText,
        DateTimeOffset sentAt,
        string? createdBy) : base(id)
    {
        DisputeId = disputeId;
        SenderId = senderId;
        SenderRole = senderRole;
        MessageText = messageText;
        SentAt = sentAt;
        CreatedAt = sentAt;
        CreatedBy = createdBy;
        IsDeleted = false;
    }

    public static Result<DisputeMessage> Create(
        Guid disputeId,
        Guid senderId,
        SenderRole senderRole,
        string messageText,
        DateTimeOffset sentAt,
        string? createdBy = null)
    {
        if (disputeId == Guid.Empty)
        {
            return Error.Validation("DisputeMessage.InvalidDispute", "Dispute ID is required");
        }

        if (senderId == Guid.Empty)
        {
            return Error.Validation("DisputeMessage.InvalidSender", "Sender ID is required");
        }

        var trimmedMessage = messageText?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(trimmedMessage))
        {
            return Error.Validation("DisputeMessage.MessageRequired", "Message text is required");
        }

        if (trimmedMessage.Length > MaxMessageLength)
        {
            return Error.Validation(
                "DisputeMessage.MessageTooLong",
                $"Message text cannot exceed {MaxMessageLength} characters");
        }

        var message = new DisputeMessage(
            Guid.NewGuid(),
            disputeId,
            senderId,
            senderRole,
            trimmedMessage,
            sentAt,
            createdBy);

        return message;
    }

    public void Delete(string? deletedBy = null)
    {
        IsDeleted = true;
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = deletedBy;
    }
}
