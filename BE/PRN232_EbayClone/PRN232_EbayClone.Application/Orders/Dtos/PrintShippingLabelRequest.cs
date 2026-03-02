using System;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record PrintShippingLabelRequest(
    string Carrier,
    string ServiceCode,
    string ServiceName,
    string PackageType,
    decimal WeightOz,
    decimal LengthIn,
    decimal WidthIn,
    decimal HeightIn,
    DateTime ShipDate,
    string PaymentMethod,
    string LabelFormat,
    string LabelPaperSize,
    decimal PageWidthIn,
    decimal PageHeightIn);
