namespace PRN232_EbayClone.Domain.Listings.ValueObjects;

public sealed record VariationSpecific(string Name, IEnumerable<string> Values);
public sealed record VariationDto(
    string Sku,
    decimal Price,
    int Quantity,
    IEnumerable<VariationSpecific> VariationSpecifics,
    IEnumerable<VariationImage>? VariationImages
);