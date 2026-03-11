using PRN232_EbayClone.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Application.Orders.Dtos
{
    public sealed class PrintShippingLabelResult
    {
        public Guid LabelId { get; }
        public Guid OrderId { get; }
        public string TrackingNumber { get; }
        public string LabelUrl { get; }
        public string LabelFileName { get; }
        public string Carrier { get; }
        public string ServiceName { get; }
        public string PackageType { get; }
        public decimal Cost { get; }
        public decimal WeightOz { get; }
        public DateTimeOffset PurchasedAt { get; }
        public DateTimeOffset? EstimatedDelivery { get; }

        public PrintShippingLabelResult(
            Guid labelId,
            Guid orderId,
            string trackingNumber,
            string labelUrl,
            string labelFileName,
            string carrier,
            string serviceName,
            string packageType,
            decimal cost,
            decimal weightOz,
            DateTimeOffset purchasedAt,
            DateTimeOffset? estimatedDelivery)
        {
            LabelId = labelId;
            OrderId = orderId;
            TrackingNumber = trackingNumber;
            LabelUrl = labelUrl;
            LabelFileName = labelFileName;
            Carrier = carrier;
            ServiceName = serviceName;
            PackageType = packageType;
            Cost = cost;
            WeightOz = weightOz;
            PurchasedAt = purchasedAt;
            EstimatedDelivery = estimatedDelivery;
        }

        public static PrintShippingLabelResult FromEntity(ShippingLabel label)
        {
            return new PrintShippingLabelResult(
                label.Id,
                label.OrderId,
                label.TrackingNumber,
                label.LabelUrl,
                label.LabelFileName,
                label.Carrier,
                label.ServiceName,
                label.PackageType,
                label.Cost.Amount,
                label.WeightOz,
                label.PurchasedAt,
                label.EstimatedDelivery);
        }
    }
}
