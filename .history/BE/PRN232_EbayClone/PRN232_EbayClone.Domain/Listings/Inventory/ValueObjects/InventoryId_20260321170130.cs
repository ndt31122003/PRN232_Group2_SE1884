namespace PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;

public sealed record InventoryId(Guid Value)
{
    public static InventoryId New() => new(Guid.NewGuid());
    public static InventoryId From(Guid id) => new(id);
    public static InventoryId From(string id) => new(Guid.Parse(id));
    public override string ToString() => Value.ToString();
}
