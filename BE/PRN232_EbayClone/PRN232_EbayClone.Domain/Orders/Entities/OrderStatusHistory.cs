using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Orders.Entities;

public class OrderStatusHistory
{
    public Guid? Id { get; private set; }
    public Guid OrderId { get; private set; }
    public OrderStatus FromStatus { get; private set; } = null!;
    public OrderStatus ToStatus { get; private set; } = null!;
    public DateTime ChangedAt { get; private set; }

    private OrderStatusHistory() { }


    public OrderStatusHistory(Guid orderId, OrderStatus from, OrderStatus to)
    {
        OrderId = orderId;
        FromStatus = from;
        ToStatus = to;
        ChangedAt = DateTime.UtcNow;
    }
}

