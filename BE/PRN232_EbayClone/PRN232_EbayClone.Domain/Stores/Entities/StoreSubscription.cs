using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Stores.Enums;
using PRN232_EbayClone.Domain.Stores.Errors;
using PRN232_EbayClone.Domain.Stores.ValueObjects;

namespace PRN232_EbayClone.Domain.Stores.Entities;

public sealed class StoreSubscription : Entity<Guid>
{
    public StoreId StoreId { get; private set; }
    public StoreType SubscriptionType { get; private set; }
    
    // Flattened from SubscriptionFees for EF mapping
    public Money MonthlyFee { get; private set; } = null!;
    public decimal FinalValueFeePercentage { get; private set; }
    public int ListingLimit { get; private set; }
    
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public SubscriptionStatus Status { get; private set; }

    private StoreSubscription(Guid id) : base(id) { }

    public static StoreSubscription Create(
        StoreId storeId,
        StoreType subscriptionType,
        SubscriptionFees fees)
    {
        return new StoreSubscription(Guid.NewGuid())
        {
            StoreId = storeId,
            SubscriptionType = subscriptionType,
            MonthlyFee = fees.MonthlyFee,
            FinalValueFeePercentage = fees.FinalValueFeePercentage,
            ListingLimit = fees.ListingLimit,
            StartDate = DateTime.UtcNow,
            Status = SubscriptionStatus.Active
        };
    }

    public Result Cancel()
    {
        if (Status == SubscriptionStatus.Cancelled)
            return StoreErrors.SubscriptionAlreadyCancelled;

        Status = SubscriptionStatus.Cancelled;
        EndDate = DateTime.UtcNow;

        return Result.Success();
    }

    public void Expire()
    {
        if (Status != SubscriptionStatus.Active)
            return;

        Status = SubscriptionStatus.Expired;
        EndDate = DateTime.UtcNow;
    }

    public bool IsActive => Status == SubscriptionStatus.Active &&
                           DateTime.UtcNow >= StartDate &&
                           (EndDate == null || DateTime.UtcNow <= EndDate.Value);
}

