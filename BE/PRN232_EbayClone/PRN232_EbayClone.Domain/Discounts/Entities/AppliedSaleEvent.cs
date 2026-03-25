using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Discounts.Entities;

public sealed class AppliedSaleEvent : Entity<Guid>
{
    public Guid OrderId { get; private set; }
    public Guid SaleEventId { get; private set; }
    public Guid? DiscountTierId { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public DateTime AppliedAt { get; private set; }

    private AppliedSaleEvent() : base(Guid.Empty) { }
    private AppliedSaleEvent(Guid id) : base(id) { }

    public static AppliedSaleEvent Create(
        Guid orderId,
        Guid saleEventId,
        Guid? discountTierId,
        decimal discountAmount)
    {
        return new AppliedSaleEvent
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            SaleEventId = saleEventId,
            DiscountTierId = discountTierId,
            DiscountAmount = discountAmount,
            AppliedAt = DateTime.UtcNow
        };
    }
}
