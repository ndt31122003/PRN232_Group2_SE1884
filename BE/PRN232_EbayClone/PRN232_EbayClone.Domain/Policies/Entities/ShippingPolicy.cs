using PRN232_EbayClone.Domain.Policies.Errors;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Stores.ValueObjects;

namespace PRN232_EbayClone.Domain.Policies.Entities;

public sealed class ShippingPolicy : AggregateRoot<Guid>
{
    public StoreId StoreId { get; private set; }
    public string Carrier { get; private set; } = null!;
    public string ServiceName { get; private set; } = null!;
    public Money Cost { get; private set; } = null!;
    public int HandlingTimeDays { get; private set; }
    public bool IsDefault { get; private set; }

    private ShippingPolicy(Guid id) : base(id) { }

    public static Result<ShippingPolicy> Create(
        StoreId storeId,
        string carrier,
        string serviceName,
        Money cost,
        int handlingTimeDays,
        bool isDefault = false)
    {
        if (string.IsNullOrWhiteSpace(carrier))
            return PolicyErrors.NameRequired;

        if (string.IsNullOrWhiteSpace(serviceName))
            return PolicyErrors.NameRequired;

        if (handlingTimeDays < 0 || handlingTimeDays > 30)
            return PolicyErrors.InvalidHandlingTime;

        var policy = new ShippingPolicy(Guid.NewGuid())
        {
            StoreId = storeId,
            Carrier = carrier,
            ServiceName = serviceName,
            Cost = cost,
            HandlingTimeDays = handlingTimeDays,
            IsDefault = isDefault
        };

        return policy;
    }

    public Result Update(string carrier, string serviceName, Money cost, int handlingTimeDays, bool isDefault)
    {
        if (string.IsNullOrWhiteSpace(carrier))
            return PolicyErrors.NameRequired;

        if (string.IsNullOrWhiteSpace(serviceName))
            return PolicyErrors.NameRequired;

        if (handlingTimeDays < 0 || handlingTimeDays > 30)
            return PolicyErrors.InvalidHandlingTime;

        Carrier = carrier;
        ServiceName = serviceName;
        Cost = cost;
        HandlingTimeDays = handlingTimeDays;
        IsDefault = isDefault;

        return Result.Success();
    }
}

