using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Seeds;

internal static class ShippingServiceSeed
{
    internal static readonly Guid UspsGroundId = Guid.Parse("6f7e3c0f-2bc6-4f1b-aa0b-4c1a9f76f950");
    internal static readonly Guid UspsPriorityFlatId = Guid.Parse("c1d3c7f4-6ac1-4a7f-8a29-6dbaf9ecbb51");
    internal static readonly Guid UspsPriorityLegalId = Guid.Parse("a1d9551e-5c5c-4ca6-9a0e-1aa855b77af7");
    internal static readonly Guid UpsGroundId = Guid.Parse("5a4af094-9a6b-4d6f-9a19-9b5360f0a6ec");
    internal static readonly Guid FedexGroundId = Guid.Parse("9e1f84fd-8c9c-459d-b2c5-bf6e47668f5d");
    private static readonly DateTime SeededAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1, 0, 0, 0), DateTimeKind.Utc);

    internal static IEnumerable<object> Services => new object[]
    {
        new
        {
            Id = UspsGroundId,
            Carrier = "USPS",
            Slug = "usps-ground",
            ServiceCode = "USPS_GROUND_ADVANTAGE",
            ServiceName = "USPS Ground Advantage",
            SavingsDescription = "On eBay you save 28%",
            CoverageDescription = "Up to $100.00",
            Notes = "Max weight 70 lb - Max dimensions 130\" (length + girth)",
            PrinterRequired = false,
            SupportsQrCode = true,
            MinEstimatedDeliveryDays = 3,
            MaxEstimatedDeliveryDays = 5,
            DeliveryWindowLabel = "Mar 28 - Apr 1",
            CreatedAt = SeededAt,
            CreatedBy = "seed",
            UpdatedAt = SeededAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = UspsPriorityFlatId,
            Carrier = "USPS",
            Slug = "usps-priority-flat",
            ServiceCode = "USPS_PRIORITY_MAIL_FLAT_RATE_ENVELOPE",
            ServiceName = "USPS Priority Mail Flat Rate Envelope",
            SavingsDescription = "On eBay you save 13%",
            CoverageDescription = "Up to $100.00",
            Notes = "Best for documents - Includes tracking",
            PrinterRequired = true,
            SupportsQrCode = false,
            MinEstimatedDeliveryDays = 2,
            MaxEstimatedDeliveryDays = 4,
            DeliveryWindowLabel = "Mar 27 - 31",
            CreatedAt = SeededAt,
            CreatedBy = "seed",
            UpdatedAt = SeededAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = UspsPriorityLegalId,
            Carrier = "USPS",
            Slug = "usps-priority-legal",
            ServiceCode = "USPS_PRIORITY_MAIL_FLAT_RATE_LEGAL_ENVELOPE",
            ServiceName = "USPS Priority Mail Flat Rate Legal Envelope",
            SavingsDescription = "On eBay you save 12%",
            CoverageDescription = "Up to $100.00",
            Notes = "Legal-size documents - Insured up to $100",
            PrinterRequired = true,
            SupportsQrCode = false,
            MinEstimatedDeliveryDays = 2,
            MaxEstimatedDeliveryDays = 4,
            DeliveryWindowLabel = "Mar 27 - 31",
            CreatedAt = SeededAt,
            CreatedBy = "seed",
            UpdatedAt = SeededAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = UpsGroundId,
            Carrier = "UPS",
            Slug = "ups-ground",
            ServiceCode = "UPS_GROUND",
            ServiceName = "UPS Ground",
            SavingsDescription = "On eBay you save 21%",
            CoverageDescription = "Up to $100.00",
            Notes = "Reliable ground service - Includes tracking",
            PrinterRequired = true,
            SupportsQrCode = false,
            MinEstimatedDeliveryDays = 3,
            MaxEstimatedDeliveryDays = 6,
            DeliveryWindowLabel = "Mar 28 - Apr 2",
            CreatedAt = SeededAt,
            CreatedBy = "seed",
            UpdatedAt = SeededAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = FedexGroundId,
            Carrier = "FedEx",
            Slug = "fedex-ground",
            ServiceCode = "FEDEX_GROUND_ECONOMY",
            ServiceName = "FedEx Ground Economy",
            SavingsDescription = "On eBay you save 18%",
            CoverageDescription = "Up to $100.00",
            Notes = "2-5 business days - Ideal for small parcels",
            PrinterRequired = true,
            SupportsQrCode = false,
            MinEstimatedDeliveryDays = 4,
            MaxEstimatedDeliveryDays = 7,
            DeliveryWindowLabel = "Mar 29 - Apr 3",
            CreatedAt = SeededAt,
            CreatedBy = "seed",
            UpdatedAt = SeededAt,
            UpdatedBy = "seed",
            IsDeleted = false
        }
    };

    internal static IEnumerable<object> BaseCosts => new object[]
    {
        new { ShippingServiceId = UspsGroundId, Amount = 11.45m, Currency = "USD" },
        new { ShippingServiceId = UspsPriorityFlatId, Amount = 8.75m, Currency = "USD" },
        new { ShippingServiceId = UspsPriorityLegalId, Amount = 9.05m, Currency = "USD" },
        new { ShippingServiceId = UpsGroundId, Amount = 15.62m, Currency = "USD" },
        new { ShippingServiceId = FedexGroundId, Amount = 14.10m, Currency = "USD" }
    };
}
