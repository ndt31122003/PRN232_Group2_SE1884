using PRN232_EbayClone.Domain.Disputes.Enums;
using PRN232_EbayClone.Domain.Disputes.Errors;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Disputes.Entities;

public class Dispute : AggregateRoot<Guid>
{
    public Guid ListingId { get; private set; }
    public string RaisedById { get; private set; } = null!;
    public string Reason { get; private set; } = null!;
    public string Status { get; private set; } = null!;

    // Navigation properties
    public Listing? Listing { get; private set; }
    private readonly List<DisputeResponse> _responses = new();
    public IReadOnlyCollection<DisputeResponse> Responses => _responses.AsReadOnly();

    private Dispute(Guid id) : base(id) { }

    public static Result<Dispute> Create(
        Guid listingId,
        string raisedById,
        string reason)
    {
        if (string.IsNullOrWhiteSpace(raisedById))
        {
            return Error.Validation("Dispute.RaisedByIdRequired", "Raised by ID là bắt buộc");
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            return DisputeErrors.ReasonRequired;
        }

        var dispute = new Dispute(Guid.NewGuid())
        {
            ListingId = listingId,
            RaisedById = raisedById,
            Reason = reason,
            Status = DisputeStatus.Open.ToString()
        };

        return Result.Success(dispute);
    }

    public Result UpdateStatus(string newStatus)
    {
        if (!Enum.TryParse<DisputeStatus>(newStatus, ignoreCase: true, out var status))
        {
            return Error.Validation("Dispute.InvalidStatus", $"Trạng thái không hợp lệ: {newStatus}");
        }

        if (status == DisputeStatus.Closed || status == DisputeStatus.Resolved)
        {
            // Can only update to closed/resolved from other statuses
            if (Status == DisputeStatus.Closed.ToString() || Status == DisputeStatus.Resolved.ToString())
            {
                return DisputeErrors.CannotUpdate;
            }
        }

        Status = newStatus;
        return Result.Success();
    }

    public Result UpdateReason(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            return DisputeErrors.ReasonRequired;
        }

        if (Status == DisputeStatus.Closed.ToString() || Status == DisputeStatus.Resolved.ToString())
        {
            return DisputeErrors.CannotUpdate;
        }

        Reason = reason;
        return Result.Success();
    }

    public bool IsClosed => Status == DisputeStatus.Closed.ToString() || Status == DisputeStatus.Resolved.ToString();

    public void AddResponse(DisputeResponse response)
    {
        _responses.Add(response);
    }
}
