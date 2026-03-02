using System;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record OrderStatusHistoryDto(
    string FromStatusCode,
    string FromStatusName,
    string ToStatusCode,
    string ToStatusName,
    DateTime ChangedAt
);
