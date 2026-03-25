namespace PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;

public sealed record InventoryReservationId(Guid Value)
{
    public static InventoryReservationId New() => new(Guid.NewGuid());
    public static InventoryReservationId From(Guid id) => new(id);
    public override string ToString() => Value.ToString();
}
