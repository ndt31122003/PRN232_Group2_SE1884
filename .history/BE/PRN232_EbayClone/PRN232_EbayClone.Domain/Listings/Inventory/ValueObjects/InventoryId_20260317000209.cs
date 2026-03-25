using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;

public sealed class InventoryId : ValueObject
{
    public Guid Value { get; }
    
    private InventoryId(Guid value) => Value = value;
    
    public static InventoryId New() => new(Guid.NewGuid());
    public static InventoryId From(Guid id) => new(id);
    public static InventoryId From(string id) => new(Guid.Parse(id));
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public override string ToString() => Value.ToString();
}
