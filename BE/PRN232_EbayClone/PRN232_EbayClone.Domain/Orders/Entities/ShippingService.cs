using System;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Domain.Orders.Entities;

public sealed class ShippingService : AggregateRoot<Guid>
{
    private ShippingService() : base(Guid.Empty)
    {
        BaseCost = null!;
    }

    private ShippingService(Guid id) : base(id)
    {
        BaseCost = null!;
    }

    public string Carrier { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string ServiceCode { get; private set; } = string.Empty;
    public string ServiceName { get; private set; } = string.Empty;
    public string SavingsDescription { get; private set; } = string.Empty;
    public string CoverageDescription { get; private set; } = string.Empty;
    public string Notes { get; private set; } = string.Empty;
    public string DeliveryWindowLabel { get; private set; } = string.Empty;
    public bool PrinterRequired { get; private set; }
    public bool SupportsQrCode { get; private set; }
    public int MinEstimatedDeliveryDays { get; private set; }
    public int MaxEstimatedDeliveryDays { get; private set; }
    public Money BaseCost { get; private set; }

    public static ShippingService Create(
        string carrier,
        string slug,
        string serviceCode,
        string serviceName,
        Money baseCost,
        int minEstimatedDeliveryDays,
        int maxEstimatedDeliveryDays,
        bool printerRequired,
        bool supportsQrCode,
        string coverageDescription,
        string savingsDescription,
        string notes,
        string deliveryWindowLabel)
    {
        return new ShippingService(Guid.NewGuid())
        {
            Carrier = carrier,
            Slug = slug,
            ServiceCode = serviceCode,
            ServiceName = serviceName,
            BaseCost = baseCost,
            MinEstimatedDeliveryDays = minEstimatedDeliveryDays,
            MaxEstimatedDeliveryDays = maxEstimatedDeliveryDays,
            PrinterRequired = printerRequired,
            SupportsQrCode = supportsQrCode,
            CoverageDescription = coverageDescription,
            SavingsDescription = savingsDescription,
            Notes = notes,
            DeliveryWindowLabel = deliveryWindowLabel,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "system",
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = "system"
        };
    }
}
