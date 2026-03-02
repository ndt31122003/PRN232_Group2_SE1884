using PRN232_EbayClone.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Application.Abstractions.Shipping
{
    public sealed class CreateLabelRequest
    {
        public Guid OrderId { get; init; }
        public string Carrier { get; init; } = string.Empty;
        public string ServiceCode { get; init; } = string.Empty;
        public string FromName { get; init; } = string.Empty;
        public string FromAddress1 { get; init; } = string.Empty;
        public string FromCity { get; init; } = string.Empty;
        public string FromPostalCode { get; init; } = string.Empty;
        public string ToName { get; init; } = string.Empty;
        public string ToAddress1 { get; init; } = string.Empty;
        public string ToCity { get; init; } = string.Empty;
        public string ToPostalCode { get; init; } = string.Empty;
        public decimal WeightOz { get; init; }
        public decimal LengthIn { get; init; }
        public decimal WidthIn { get; init; }
        public decimal HeightIn { get; init; }
        public DateTimeOffset ShipDate { get; init; }
    }

    public sealed class CreateLabelResponse
    {
        public string LabelDocumentId { get; init; } = string.Empty;
        public string LabelUrl { get; init; } = string.Empty;
        public string TrackingNumber { get; init; } = string.Empty;
        public Money Cost { get; init; } = null!;
        public Money Insurance { get; init; } = null!;
        public string PackageType { get; init; } = string.Empty;
        public decimal WeightOz { get; init; }
        public decimal LengthIn { get; init; }
        public decimal WidthIn { get; init; }
        public decimal HeightIn { get; init; }
    }

    public interface IShippingProvider
    {
        Task<CreateLabelResponse> CreateShippingLabelAsync(
            string fromName,
            string fromStreet1,
            string fromStreet2,
            string fromCity,
            string fromState,
            string fromPostalCode,
            string fromCountry,
            string toName,
            string toStreet1,
            string toStreet2,
            string toCity,
            string toState,
            string toPostalCode,
            string toCountry,
            decimal weightOz,
            decimal lengthIn,
            decimal widthIn,
            decimal heightIn,
            Money insuranceAmount);
    }
}
