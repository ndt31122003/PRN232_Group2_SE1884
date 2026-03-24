using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Domain.Listings.Entities;

public abstract class Listing(Guid id) : AggregateRoot<Guid>(id)
{
    public ListingFormat Format { get; protected set; }
    public ListingStatus Status { get; protected set; }
    public string Title { get; protected set; } = null!;
    public string Sku { get; protected set; } = null!;
    public string ListingDescription { get; protected set; } = null!;
    public Guid CategoryId { get; protected set; }
    public Guid? ConditionId { get; protected set; }
    public string ConditionDescription { get; protected set; } = null!;
    public DateTime? ScheduledStartTime { get; protected set; }
    public DateTime? DraftExpiredAt { get; protected set; }
    public DateTime? StartDate { get; protected set; }
    public DateTime? EndDate { get; protected set; }

    protected readonly HashSet<ItemSpecific> _itemSpecifics = [];
    public IReadOnlyCollection<ItemSpecific> ItemSpecifics => _itemSpecifics;
    public Guid? ShippingPolicyId { get; protected set; }
    public Guid? ReturnPolicyId { get; protected set; }
    public int WatchersCount { get; protected set; }
    public Duration Duration { get; protected set; }

    protected readonly HashSet<ListingImage> _images = [];
    public IReadOnlyCollection<ListingImage> Images => _images;
    public DateTime? LastPriceChangeDate { get; protected set; }
    public abstract decimal GetEstimatedValue();

    public Result UpdateCommon(
        string title,
        string sku,
        string listingDescription,
        Guid categoryId,
        Guid? conditionId,
        string conditionDescription,
        IEnumerable<ItemSpecific> itemSpecifics,
        IEnumerable<ListingImage> listingImages,
        Guid? shippingPolicyId,
        Guid? returnPolicyId)
    {
        Title = title;
        Sku = sku;
        ListingDescription = listingDescription;
        CategoryId = categoryId;
        ConditionId = conditionId;
        ConditionDescription = conditionDescription;
        ShippingPolicyId = shippingPolicyId;
        ReturnPolicyId = returnPolicyId;
        _itemSpecifics.Clear();
        _itemSpecifics.UnionWith(itemSpecifics);

        _images.Clear();
        foreach (var img in listingImages)
        {
            var addImageResult = AddImage(img.Url, img.IsPrimary);
            if (addImageResult.IsFailure) return addImageResult.Error;
        }

        return Result.Success();
    }

    public Result ChangeFormat(ListingFormat newFormat)
    {
        if (Status != ListingStatus.Draft)
        {
            return Error.Failure(
                "Listing.FormatChangeNotAllowed",
                "Cannot change format when listing is active.");
        }

        if (Format == newFormat)
            return Result.Success();

        ClearAllForFormatChange();

        Format = newFormat;

        return Result.Success();
    }

    protected virtual void ClearAllForFormatChange()
    {
        _itemSpecifics.Clear();
    }

    public Result Schedule(DateTime scheduledTime)
    {
        if (Status != ListingStatus.Draft)
            return Error.Failure(
                "Listing.ScheduleNotAllowed",
                "Only draft listings can be scheduled.");

        if (scheduledTime <= DateTime.UtcNow)
            return Error.Failure(
                "Listing.InvalidScheduledTime",
                "Scheduled time must be in the future.");

        ScheduledStartTime = scheduledTime;
        Status = ListingStatus.Scheduled;

        return Result.Success();
    }

    public Result Activate()
    {
        if (ScheduledStartTime is not null && ScheduledStartTime > DateTime.UtcNow)
            return Error.Failure(
                "Listing.NotDueYet",
                "Scheduled time has not arrived yet.");

        Status = ListingStatus.Active;
        ScheduledStartTime = null;
        StartDate = DateTime.UtcNow;

        if (Duration == Duration.Gtc)
            EndDate = null;
        else
            EndDate = StartDate.Value.AddDays((int)Duration);

        return Result.Success();
    }

    public Result End()
    {
        if (Status != ListingStatus.Active)
            return Error.Failure(
                "Listing.EndNotAllowed",
                "Only active listings can be ended.");
        Status = ListingStatus.Ended;
        return Result.Success();
    }

    public Result Draft()
    {
        if (Status != ListingStatus.Draft)
            return Error.Failure(
                "Listing.DraftNotAllowed",
                "Only draft listings can be set to draft.");
        DraftExpiredAt = DateTime.UtcNow.AddDays(75);
        return Result.Success();
    }

    public Result AddImage(string url, bool isPrimary = false)
    {
        if (_images.Count > 25)
            return Error.Failure("Listing.MaxImages", "Cannot upload more than 25 images.");

        if (isPrimary && _images.Any(i => i.IsPrimary))
            return Error.Failure("Listing.PrimaryImageExists", "Primary image already set.");

        _images.Add(new ListingImage(url, isPrimary));
        return Result.Success();
    }

}
