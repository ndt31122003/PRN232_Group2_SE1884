using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Listings.Entities;

public class Variation
{
    public int Id { get; private set; }
    public string Sku { get; private set; } = null!;
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }

    private readonly List<VariationSpecific> _variationSpecifics = [];
    public IReadOnlyCollection<VariationSpecific> VariationSpecifics => _variationSpecifics;

    private readonly List<VariationImage> _images = [];
    public IReadOnlyCollection<VariationImage> Images => _images;

    public static Result<Variation> Create(
        string sku,
        decimal price,
        IEnumerable<VariationSpecific> variationSpecifics,
        IEnumerable<VariationImage> images,
        int quantity = 1)
    {
        var variation = new Variation
        {
            Sku = sku,
            Price = price,
            Quantity = quantity
        };
        variation._variationSpecifics.AddRange(variationSpecifics);

        foreach (var img in images)
        {
            var addImageResult = variation.AddImage(img.Url, img.IsPrimary);
            if (addImageResult.IsFailure) return addImageResult.Error;
        }

        return variation;
    }

    public Result AddImage(string url, bool isPrimary)
    {
        if (_images.Count > 25)
            return Error.Failure("Variation.MaxImages", "Cannot upload more than 25 images.");

        if (isPrimary && _images.Any(i => i.IsPrimary))
            return Error.Failure("Variation.PrimaryImageExists", "Primary image already set.");

        _images.Add(new VariationImage(url, isPrimary));
        return Result.Success();
    }
    public void ClearSpecifics()
    {
        _variationSpecifics.Clear();
    }
}
