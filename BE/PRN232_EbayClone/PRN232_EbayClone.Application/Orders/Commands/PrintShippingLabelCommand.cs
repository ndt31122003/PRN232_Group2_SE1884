using System;
using System.IO;
using System.Linq;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Shipping;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Commands;

public sealed record PrintShippingLabelCommand(
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
    string PaymentMethod,
    string LabelFormat,
    string LabelPaperSize,
    decimal PageWidthIn,
    decimal PageHeightIn
) : ICommand<PrintShippingLabelResult>;

public sealed class PrintShippingLabelCommandHandler : ICommandHandler<PrintShippingLabelCommand, PrintShippingLabelResult>
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
    private readonly IShippingProvider _shippingProvider;
    private readonly IUnitOfWork _unitOfWork;

    public PrintShippingLabelCommandHandler(
        IOrderRepository orderRepository,
        IShippingLabelRepository shippingLabelRepository,
        IShippingLabelRenderer shippingLabelRenderer,
        IShippingProvider shippingProvider,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _shippingLabelRepository = shippingLabelRepository;
        _shippingLabelRenderer = shippingLabelRenderer;
        _shippingProvider = shippingProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PrintShippingLabelResult>> Handle(
        PrintShippingLabelCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return Result.Failure<PrintShippingLabelResult>(
                Error.Failure("Order.NotFound", "Order not found."));
        }

        var buyerName = order.Buyer?.FullName ?? order.Buyer?.Username ?? "Unknown Recipient";
        var buyerStreet = "742 Evergreen Terrace";
        var buyerCity = "Springfield";
        var buyerState = "IL";
        var buyerPostalCode = "62704";
        var buyerCountry = "US";

        var providerResponse = await _shippingProvider.CreateShippingLabelAsync(
            fromName: DefaultSenderName,
            fromStreet1: DefaultSenderStreet,
            fromStreet2: string.Empty,
            fromCity: DefaultSenderCity,
            fromState: DefaultSenderState,
            fromPostalCode: DefaultSenderPostalCode,
            fromCountry: DefaultSenderCountry,
            toName: buyerName,
            toStreet1: buyerStreet,
            toStreet2: string.Empty,
            toCity: buyerCity,
            toState: buyerState,
            toPostalCode: buyerPostalCode,
            toCountry: buyerCountry,
            weightOz: request.WeightOz,
            lengthIn: request.LengthIn,
            widthIn: request.WidthIn,
            heightIn: request.HeightIn,
            insuranceAmount: order.Total);

        var (pageWidthIn, pageHeightIn) = NormalizePageSize(request.PageWidthIn, request.PageHeightIn);

        var previousLabel = await _shippingLabelRepository.GetLatestForOrderAsync(order.Id, cancellationToken);
        var shippingServiceId = previousLabel?.ShippingServiceId ?? Guid.Empty;
        var requestedServiceName = string.IsNullOrWhiteSpace(request.ServiceName)
            ? request.ServiceCode
            : request.ServiceName;
        var serviceName = previousLabel?.ServiceName ?? requestedServiceName;
        var labelFileName = BuildLabelFileName(providerResponse.TrackingNumber);
        var shippingLabel = ShippingLabel.Create(
            orderId: order.Id,
            shippingServiceId: shippingServiceId,
            carrier: request.Carrier,
            serviceCode: request.ServiceCode,
            serviceName: serviceName,
            trackingNumber: providerResponse.TrackingNumber,
            labelUrl: providerResponse.LabelUrl,
            labelFileName: labelFileName,
            cost: providerResponse.Cost,
            insurance: providerResponse.Insurance,
            packageType: string.IsNullOrWhiteSpace(providerResponse.PackageType) ? request.PackageType : providerResponse.PackageType,
            weightOz: providerResponse.WeightOz > 0 ? providerResponse.WeightOz : request.WeightOz,
            lengthIn: providerResponse.LengthIn > 0 ? providerResponse.LengthIn : request.LengthIn,
            widthIn: providerResponse.WidthIn > 0 ? providerResponse.WidthIn : request.WidthIn,
            heightIn: providerResponse.HeightIn > 0 ? providerResponse.HeightIn : request.HeightIn,
            purchasedAt: DateTimeOffset.UtcNow,
            estimatedDelivery: null,
            labelDocumentId: providerResponse.LabelDocumentId);

        _shippingLabelRepository.Add(shippingLabel);

        var shippedAt = DateTimeOffset.UtcNow;

        foreach (var item in order.Items)
        {
            var shipmentResult = order.AddShipment(
                item.Id,
                request.Carrier,
                providerResponse.TrackingNumber,
        shippedAt,
                shippingLabel.Id);

            if (shipmentResult.IsFailure)
            {
                return Result.Failure<PrintShippingLabelResult>(shipmentResult.Error);
            }
        }

        if (order.HasShipmentsForAllItems())
        {
            var paidAndShippedStatus = await _orderRepository.GetStatusByCodeAsync(OrderStatusCodes.PaidAndShipped, cancellationToken);
            if (paidAndShippedStatus is not null)
            {
                var changeStatusResult = order.ChangeStatus(paidAndShippedStatus, OrderRoles.Seller);
                if (changeStatusResult.IsFailure)
                {
                    return Result.Failure<PrintShippingLabelResult>(changeStatusResult.Error);
                }
            }
        }

        var renderModel = new ShippingLabelRenderModel
        {
            OrderNumber = order.OrderNumber,
            RecipientName = buyerName,
            RecipientAddress = $"{buyerStreet}\n{buyerCity}, {buyerState} {buyerPostalCode}",
            SenderName = DefaultSenderName,
            SenderAddress = $"{DefaultSenderStreet}\n{DefaultSenderCity}, {DefaultSenderState} {DefaultSenderPostalCode}",
            CarrierName = shippingLabel.Carrier,
            ServiceName = string.IsNullOrWhiteSpace(shippingLabel.ServiceName) ? request.ServiceCode : shippingLabel.ServiceName,
            PackageType = shippingLabel.PackageType,
            WeightOz = shippingLabel.WeightOz,
            LengthIn = shippingLabel.LengthIn,
            WidthIn = shippingLabel.WidthIn,
            HeightIn = shippingLabel.HeightIn,
            Cost = shippingLabel.Cost.Amount,
            CostCurrency = shippingLabel.Cost.Currency,
            InsuranceAmount = shippingLabel.Insurance.Amount,
            InsuranceCurrency = shippingLabel.Insurance.Currency,
            TrackingNumber = shippingLabel.TrackingNumber,
            BarcodeValue = shippingLabel.TrackingNumber,
            ShipDate = request.ShipDate,
            LabelFormat = string.IsNullOrWhiteSpace(request.LabelFormat) ? "pdf" : request.LabelFormat.Trim(),
            LabelPaperSize = string.IsNullOrWhiteSpace(request.LabelPaperSize) ? "4x6" : request.LabelPaperSize.Trim(),
            PageWidthIn = pageWidthIn,
            PageHeightIn = pageHeightIn
        };

        var pdfBytes = await _shippingLabelRenderer.RenderPdfAsync(renderModel, cancellationToken);

        var storageDirectory = Path.Combine(AppContext.BaseDirectory, "storage", "shipping-labels");
        Directory.CreateDirectory(storageDirectory);

        var absolutePath = Path.Combine(storageDirectory, labelFileName);
        await File.WriteAllBytesAsync(absolutePath, pdfBytes, cancellationToken);

        var relativePath = Path.Combine("storage", "shipping-labels", labelFileName).Replace('\\', '/');
        shippingLabel.UpdateLabelUrl("/" + relativePath, labelFileName);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(PrintShippingLabelResult.FromEntity(shippingLabel));
    }

    private static (decimal Width, decimal Height) NormalizePageSize(decimal widthIn, decimal heightIn)
    {
        const decimal defaultWidth = 4m;
        const decimal defaultHeight = 6m;

        var sanitizedWidth = widthIn > 0 ? widthIn : defaultWidth;
        var sanitizedHeight = heightIn > 0 ? heightIn : defaultHeight;

        return (sanitizedWidth, sanitizedHeight);
    }

    private static string BuildLabelFileName(string trackingNumber)
    {
        var sanitizedTracking = string.IsNullOrWhiteSpace(trackingNumber)
            ? "label"
            : new string(trackingNumber.Where(char.IsLetterOrDigit).ToArray());

        return $"{sanitizedTracking}_{DateTimeOffset.UtcNow:yyyyMMddHHmmss}.pdf";
    }
}
