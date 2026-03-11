using System;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Domain.Orders.Entities;

public sealed class ShippingLabel
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ShippingServiceId { get; private set; }
    public string Carrier { get; private set; } = string.Empty;
    public string ServiceCode { get; private set; } = string.Empty;
    public string ServiceName { get; private set; } = string.Empty;
    public string TrackingNumber { get; private set; } = string.Empty;
    public string LabelUrl { get; private set; } = string.Empty;
    public string LabelFileName { get; private set; } = string.Empty;
    public Money Cost { get; private set; } = null!;
    public Money Insurance { get; private set; } = null!;
    public string PackageType { get; private set; } = string.Empty;
    public decimal WeightOz { get; private set; }
    public decimal LengthIn { get; private set; }
    public decimal WidthIn { get; private set; }
    public decimal HeightIn { get; private set; }
    public DateTimeOffset PurchasedAt { get; private set; }
    public DateTimeOffset? EstimatedDelivery { get; private set; }
    public string LabelDocumentId { get; private set; } = string.Empty;
    public bool IsVoided { get; private set; }
    public DateTimeOffset? VoidedAt { get; private set; }
    public string? VoidReason { get; private set; }

    private ShippingLabel()
    {
    }

    private ShippingLabel(
        Guid id,
        Guid orderId,
        Guid shippingServiceId,
        string carrier,
        string serviceCode,
        string serviceName,
        string trackingNumber,
        string labelUrl,
        string labelFileName,
        Money cost,
        Money insurance,
        string packageType,
        decimal weightOz,
        decimal lengthIn,
        decimal widthIn,
        decimal heightIn,
        DateTimeOffset purchasedAt,
        DateTimeOffset? estimatedDelivery,
        string labelDocumentId)
    {
        Id = id;
        OrderId = orderId;
        ShippingServiceId = shippingServiceId;
        Carrier = carrier;
        ServiceCode = serviceCode;
        ServiceName = serviceName;
        TrackingNumber = trackingNumber;
        LabelUrl = labelUrl;
        LabelFileName = labelFileName;
        Cost = cost;
        Insurance = insurance;
        PackageType = packageType;
        WeightOz = weightOz;
        LengthIn = lengthIn;
        WidthIn = widthIn;
        HeightIn = heightIn;
        PurchasedAt = purchasedAt;
        EstimatedDelivery = estimatedDelivery;
        LabelDocumentId = labelDocumentId;
    }

    public static ShippingLabel Create(

        Guid orderId,
        Guid shippingServiceId,
        string carrier,
        string serviceCode,
        string serviceName,
        string trackingNumber,
        string labelUrl,
        string labelFileName,
        Money cost,
        Money insurance,
        string packageType,
        decimal weightOz,
        decimal lengthIn,
        decimal widthIn,
        decimal heightIn,
        DateTimeOffset purchasedAt,
        DateTimeOffset? estimatedDelivery,
        string labelDocumentId = "")
    {
        return new ShippingLabel(
            Guid.NewGuid(),
            orderId,
            shippingServiceId,
            carrier,
            serviceCode,
            serviceName,
            trackingNumber,
            labelUrl,
            labelFileName,
            cost,
            insurance,
            packageType,
            weightOz,
            lengthIn,
            widthIn,
            heightIn,
            purchasedAt,
            estimatedDelivery,
            labelDocumentId)
        {
            IsVoided = false,
            VoidedAt = null,
            VoidReason = null
        };
    }

    public void UpdateLabelUrl(string labelUrl, string labelFileName)
    {
        LabelUrl = labelUrl;
        LabelFileName = labelFileName;
    }

    public void UpdateTracking(string trackingNumber, DateTimeOffset? estimatedDelivery)
    {
        TrackingNumber = trackingNumber;
        EstimatedDelivery = estimatedDelivery;
    }

    public Result Void(string? reason)
    {
        if (IsVoided)
        {
            return Result.Success();
        }

        IsVoided = true;
        VoidedAt = DateTimeOffset.UtcNow;
        VoidReason = string.IsNullOrWhiteSpace(reason)
            ? null
            : reason.Trim();

        return Result.Success();
    }
}
