namespace PRN232_EbayClone.Domain.Notifications.Entities;

public sealed class Notification
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }   // recipient
    public string Type { get; private set; } = null!; // "NewBid" | "NewOffer"
    public string Title { get; private set; } = null!;
    public string Message { get; private set; } = null!;
    public Guid? ReferenceId { get; private set; } // ListingId or OfferId
    public bool IsRead { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Notification() { }

    public static Notification Create(
        Guid userId,
        string type,
        string title,
        string message,
        Guid? referenceId = null)
    {
        return new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = type,
            Title = title,
            Message = message,
            ReferenceId = referenceId,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void MarkAsRead() => IsRead = true;
}
