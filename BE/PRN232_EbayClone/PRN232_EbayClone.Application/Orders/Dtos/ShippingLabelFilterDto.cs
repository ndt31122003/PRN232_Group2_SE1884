using System;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed class ShippingLabelFilterDto
{
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public int? Limit { get; init; }
}
