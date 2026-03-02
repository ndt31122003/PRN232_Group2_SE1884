using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record OrderItemDto(
    Guid Id,
    Guid ListingId,
    Guid? VariationId,
    string Title,
    string ImageUrl,
    string Sku,
    int Quantity,
    decimal? UnitPrice,
    decimal? TotalPrice
);

