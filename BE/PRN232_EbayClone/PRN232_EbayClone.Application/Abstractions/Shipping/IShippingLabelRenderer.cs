using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Application.Abstractions.Shipping
{
    public sealed class ShippingLabelRenderModel
    {
        public string OrderNumber { get; set; } = default!;
        public string RecipientName { get; set; } = default!;
        public string RecipientAddress { get; set; } = default!;
        public string SenderName { get; set; } = default!;
        public string SenderAddress { get; set; } = default!;
        public string CarrierName { get; set; } = default!;
        public string ServiceName { get; set; } = string.Empty;
        public string PackageType { get; set; } = string.Empty;
        public decimal WeightOz { get; set; }
        public decimal LengthIn { get; set; }
        public decimal WidthIn { get; set; }
        public decimal HeightIn { get; set; }
        public decimal Cost { get; set; }
        public string CostCurrency { get; set; } = "USD";
        public decimal InsuranceAmount { get; set; }
        public string InsuranceCurrency { get; set; } = "USD";
        public string TrackingNumber { get; set; } = default!;
        public string BarcodeValue { get; set; } = default!;
        public DateTime ShipDate { get; set; }
        public string LabelFormat { get; set; } = "pdf";
        public string LabelPaperSize { get; set; } = "4x6";
        public decimal PageWidthIn { get; set; } = 4m;
        public decimal PageHeightIn { get; set; } = 6m;
    }
    public interface IShippingLabelRenderer
    {
        Task<byte[]> RenderPdfAsync(ShippingLabelRenderModel model, CancellationToken ct);
    }

}
