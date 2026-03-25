using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Abstractions.Shipping;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Commands;

public sealed record PreviewShippingLabelCommand(
    Guid OrderId,
    string Carrier,
    string ServiceCode,
    string ServiceName,
    string PackageType,
    decimal WeightOz,
    decimal LengthIn,
    decimal WidthIn,
    decimal HeightIn,
    DateTime ShipDate,
    string LabelFormat,
    string LabelPaperSize,
    decimal PageWidthIn,
    decimal PageHeightIn
) : ICommand<PreviewShippingLabelResult>;

public sealed class PreviewShippingLabelCommandHandler : ICommandHandler<PreviewShippingLabelCommand, PreviewShippingLabelResult>
{
    private const string DefaultSenderName = "EbayClone Fulfillment Center";
    private const string DefaultSenderStreet = "123 Fulfillment Road";
    private const string DefaultSenderCity = "Ho Chi Minh City";
    private const string DefaultSenderState = "HCM";
    private const string DefaultSenderPostalCode = "700000";
    private const string DefaultSenderCountry = "VN";

    private readonly IOrderRepository _orderRepository;
    private readonly IShippingLabelRepository _shippingLabelRepository;
    private readonly IShippingLabelRenderer _shippingLabelRenderer;

    public PreviewShippingLabelCommandHandler(
        IOrderRepository orderRepository,
        IShippingLabelRepository shippingLabelRepository,
        IShippingLabelRenderer shippingLabelRenderer)
    {
        _orderRepository = orderRepository;
        _shippingLabelRepository = shippingLabelRepository;
        _shippingLabelRenderer = shippingLabelRenderer;
    }

    public async Task<Result<PreviewShippingLabelResult>> Handle(
        PreviewShippingLabelCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return Result.Failure<PreviewShippingLabelResult>(
                Error.Failure("Order.NotFound", "Order not found."));
        }

        var latestLabel = await _shippingLabelRepository.GetLatestForOrderAsync(order.Id, cancellationToken);
        var (pageWidthIn, pageHeightIn) = NormalizePageSize(request.PageWidthIn, request.PageHeightIn);

        var recipientName = order.Buyer?.FullName ?? order.Buyer?.Username ?? "Unknown Recipient";
        var recipientStreet = "742 Evergreen Terrace";
        var recipientCity = "Springfield";
        var recipientState = "IL";
        var recipientPostalCode = "62704";
        var trackingNumber = BuildPreviewTrackingNumber(order, latestLabel);

        var renderModel = new ShippingLabelRenderModel
        {
            OrderNumber = order.OrderNumber,
            RecipientName = recipientName,
            RecipientAddress = $"{recipientStreet}\n{recipientCity}, {recipientState} {recipientPostalCode}",
            SenderName = DefaultSenderName,
            SenderAddress = $"{DefaultSenderStreet}\n{DefaultSenderCity}, {DefaultSenderState} {DefaultSenderPostalCode}",
            CarrierName = request.Carrier,
            ServiceName = string.IsNullOrWhiteSpace(request.ServiceName) ? request.ServiceCode : request.ServiceName,
            PackageType = request.PackageType,
            WeightOz = request.WeightOz,
            LengthIn = request.LengthIn,
            WidthIn = request.WidthIn,
            HeightIn = request.HeightIn,
            Cost = order.ShippingCost.Amount,
            CostCurrency = order.ShippingCost.Currency,
            InsuranceAmount = order.Total.Amount,
            InsuranceCurrency = order.Total.Currency,
            TrackingNumber = trackingNumber,
            BarcodeValue = trackingNumber,
            ShipDate = request.ShipDate,
            LabelFormat = string.IsNullOrWhiteSpace(request.LabelFormat) ? "pdf" : request.LabelFormat.Trim(),
            LabelPaperSize = string.IsNullOrWhiteSpace(request.LabelPaperSize) ? "4x6" : request.LabelPaperSize.Trim(),
            PageWidthIn = pageWidthIn,
            PageHeightIn = pageHeightIn
        };

        var pdfBytes = await _shippingLabelRenderer.RenderPdfAsync(renderModel, cancellationToken);

        var fileName = BuildPreviewFileName(order);
        var base64 = Convert.ToBase64String(pdfBytes);

        var previewResult = new PreviewShippingLabelResult(
            fileName,
            "application/pdf",
            base64);

        return Result.Success(previewResult);
    }

    private static (decimal Width, decimal Height) NormalizePageSize(decimal widthIn, decimal heightIn)
    {
        const decimal defaultWidth = 4m;
        const decimal defaultHeight = 6m;

        var sanitizedWidth = widthIn > 0 ? widthIn : defaultWidth;
        var sanitizedHeight = heightIn > 0 ? heightIn : defaultHeight;

        return (sanitizedWidth, sanitizedHeight);
    }

    private static string BuildPreviewTrackingNumber(Order order, ShippingLabel? latestLabel)
    {
        var trackingNumber = latestLabel?.TrackingNumber;
        if (!string.IsNullOrWhiteSpace(trackingNumber))
        {
            return trackingNumber;
        }

        var identifier = string.IsNullOrWhiteSpace(order.OrderNumber)
            ? order.Id.ToString("N")
            : order.OrderNumber;

        var sanitized = new string(identifier.Where(char.IsLetterOrDigit).ToArray());
        if (string.IsNullOrWhiteSpace(sanitized))
        {
            sanitized = order.Id.ToString("N");
        }

        var previewSuffix = DateTimeOffset.UtcNow.ToString("HHmmss");
        return $"PREVIEW-{sanitized}-{previewSuffix}";
    }

    private static string BuildPreviewFileName(Order order)
    {
        var identifier = string.IsNullOrWhiteSpace(order.OrderNumber)
            ? order.Id.ToString("N")
            : order.OrderNumber;

        var sanitized = new string(identifier.Where(char.IsLetterOrDigit).ToArray());
        if (string.IsNullOrWhiteSpace(sanitized))
        {
            sanitized = order.Id.ToString("N");
        }

        return $"{sanitized}_preview_{DateTimeOffset.UtcNow:yyyyMMddHHmmss}.pdf";
    }
}
