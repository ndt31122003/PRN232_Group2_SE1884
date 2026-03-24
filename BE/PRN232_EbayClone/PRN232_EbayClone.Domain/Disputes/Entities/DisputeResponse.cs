using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Disputes.Entities;

public class DisputeResponse : Entity<Guid>
{
    public Guid DisputeId { get; private set; }
    public Guid ResponderId { get; private set; }
    public string Message { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    // Navigation properties
    public Dispute? Dispute { get; private set; }

    private DisputeResponse(Guid id) : base(id) { }

    public static DisputeResponse Create(
        Guid disputeId,
        Guid responderId,
        string message)
    {
        return new DisputeResponse(Guid.NewGuid())
        {
            DisputeId = disputeId,
            ResponderId = responderId,
            Message = message,
            CreatedAt = DateTime.UtcNow
        };
    }
}
