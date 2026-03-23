using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;

public sealed class InventoryReservationId : ValueObject
{
    public Guid Value { get; }
    
    private InventoryReservationId(Guid value) => Value = value;
    
    public static InventoryReservationId New() => new(Guid.NewGuid());
    public static InventoryReservationId From(Guid id) => new(id);
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
