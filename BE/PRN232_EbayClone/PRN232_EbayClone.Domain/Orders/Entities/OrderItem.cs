using System;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Domain.Orders.Entities;
public class OrderItem(Guid id) : AggregateRoot<Guid>(id)
{
    public Guid ListingId { get; private set; }
    public Guid? VariationId { get; private set; }
    public Guid? CategoryId { get; private set; }
    public string Title { get; private set; } = null!;
    public string ImageUrl { get; private set; } = null!;

    public string Sku { get; private set; } = null!;
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; } = null!;
    public Money TotalPrice { get; private set; } = null!;
    public static OrderItem Create(
        Guid listingId,
        Guid? variationId,
        Guid? categoryId,
        string imageUrl,
        string title,
        string sku,
        int quantity,
        Money unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        if (unitPrice.Amount <= 0)
            throw new ArgumentException("Unit price must be greater than zero.", nameof(unitPrice));
        var orderItem = new OrderItem(Guid.NewGuid())
        {
            ListingId = listingId,
            VariationId = variationId == Guid.Empty ? null : variationId,
            CategoryId = categoryId,
            Title = title,
            ImageUrl = imageUrl,
            Sku = sku,
            Quantity = quantity,
            UnitPrice = unitPrice,
            TotalPrice = Money.Create(unitPrice.Amount * quantity, unitPrice.Currency).Value
        };
        return orderItem;
    }

}
