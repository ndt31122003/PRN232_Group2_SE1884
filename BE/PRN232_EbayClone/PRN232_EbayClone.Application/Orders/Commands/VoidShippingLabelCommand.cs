using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Commands;

public sealed record VoidShippingLabelCommand(
    Guid OrderId,
    Guid LabelId,
    Guid SellerId,
    string? Reason) : ICommand<ShippingLabelSummaryDto>;

public sealed class VoidShippingLabelCommandHandler : ICommandHandler<VoidShippingLabelCommand, ShippingLabelSummaryDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IShippingLabelRepository _shippingLabelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VoidShippingLabelCommandHandler(
        IOrderRepository orderRepository,
        IShippingLabelRepository shippingLabelRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _shippingLabelRepository = shippingLabelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ShippingLabelSummaryDto>> Handle(VoidShippingLabelCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null || order.SellerId != request.SellerId)
        {
            return Result.Failure<ShippingLabelSummaryDto>(
                Error.Failure("Order.NotFound", "The order could not be found."));
        }

        var label = await _shippingLabelRepository.GetByIdAsync(request.LabelId, cancellationToken);
        if (label is null || label.OrderId != order.Id)
        {
            return Result.Failure<ShippingLabelSummaryDto>(
                Error.Failure("ShippingLabel.NotFound", "The shipping label could not be found."));
        }

        var voidResult = label.Void(request.Reason);
        if (voidResult.IsFailure)
        {
            return Result.Failure<ShippingLabelSummaryDto>(voidResult.Error);
        }

        _shippingLabelRepository.Update(label);

        var shipmentsToRemove = order.ItemShipments
            .Where(shipment => shipment.ShippingLabelId == label.Id)
            .Select(shipment => shipment.Id)
            .ToList();

        foreach (var shipmentId in shipmentsToRemove)
        {
            var removal = order.RemoveShipment(shipmentId);
            if (removal.IsFailure)
            {
                return Result.Failure<ShippingLabelSummaryDto>(removal.Error);
            }
        }

        if (!order.HasShipmentsForAllItems())
        {
            var awaitingShipmentStatus = await _orderRepository.GetStatusByCodeAsync(OrderStatusCodes.AwaitingShipment, cancellationToken);
            if (awaitingShipmentStatus is not null)
            {
                var statusResult = order.ChangeStatus(awaitingShipmentStatus, OrderRoles.Seller);
                if (statusResult.IsFailure)
                {
                    return Result.Failure<ShippingLabelSummaryDto>(statusResult.Error);
                }
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = new ShippingLabelSummaryDto(
            label.Id,
            order.Id,
            order.OrderNumber,
            label.Carrier,
            label.ServiceName,
            label.TrackingNumber,
            label.PurchasedAt,
            new MoneyDto(label.Cost.Amount, label.Cost.Currency),
            label.LabelUrl,
            label.LabelFileName,
            label.IsVoided,
            label.VoidedAt,
            label.VoidReason);

        return Result.Success(dto);
    }
}
