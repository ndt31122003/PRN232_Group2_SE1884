namespace PRN232_EbayClone.Domain.Stores.ValueObjects;

public readonly record struct StoreId(Guid Value)
{
    public static StoreId New() => new(Guid.NewGuid());
    public static StoreId From(Guid value) => new(value);
    public override string ToString() => Value.ToString();
}

