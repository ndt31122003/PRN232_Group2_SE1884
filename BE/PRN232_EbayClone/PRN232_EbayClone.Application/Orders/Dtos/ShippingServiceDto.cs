using System;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record ShippingServiceDto(
    Guid Id,
    string Carrier,
    string Slug,
    string ServiceCode,
    string ServiceName,
    MoneyDto BaseCost,
    int MinEstimatedDeliveryDays,
    int MaxEstimatedDeliveryDays,
    bool PrinterRequired,
    bool SupportsQrCode,
    string CoverageDescription,
    string SavingsDescription,
    string Notes,
    string DeliveryWindowLabel);
