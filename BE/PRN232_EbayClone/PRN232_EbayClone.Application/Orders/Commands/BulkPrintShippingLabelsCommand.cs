using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Commands;

public sealed record BulkPrintShippingLabelsCommand(
    Guid SellerId,
    IReadOnlyList<Guid> OrderIds,
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
) : ICommand<BulkPrintShippingLabelsResult>;

public sealed class BulkPrintShippingLabelsCommandValidator : AbstractValidator<BulkPrintShippingLabelsCommand>
{
    public BulkPrintShippingLabelsCommandValidator()
    {
        RuleFor(x => x.SellerId)
            .NotEmpty();

        RuleFor(x => x.OrderIds)
            .NotNull().WithMessage("Select at least one order.")
            .Must(ids => ids.Any()).WithMessage("Select at least one order.");

        RuleForEach(x => x.OrderIds)
            .NotEmpty().WithMessage("Order id is required.");

        RuleFor(x => x.Carrier)
            .NotEmpty().WithMessage("Carrier is required.");

        RuleFor(x => x.ServiceCode)
            .NotEmpty().WithMessage("Service code is required.");

        RuleFor(x => x.PackageType)
            .NotEmpty().WithMessage("Package type is required.");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("Payment method is required.");

        RuleFor(x => x.WeightOz)
            .GreaterThan(0).WithMessage("Package weight must be greater than zero.");

        RuleFor(x => x.LengthIn)
            .GreaterThan(0).WithMessage("Package length must be greater than zero.");

        RuleFor(x => x.WidthIn)
            .GreaterThan(0).WithMessage("Package width must be greater than zero.");

        RuleFor(x => x.HeightIn)
            .GreaterThan(0).WithMessage("Package height must be greater than zero.");

        RuleFor(x => x.ShipDate)
            .Must(date => date != default)
            .WithMessage("Ship date is required.");
    }
}

public sealed class BulkPrintShippingLabelsCommandHandler : ICommandHandler<BulkPrintShippingLabelsCommand, BulkPrintShippingLabelsResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISender _sender;

    public BulkPrintShippingLabelsCommandHandler(IOrderRepository orderRepository, ISender sender)
    {
        _orderRepository = orderRepository;
        _sender = sender;
    }

    public async Task<Result<BulkPrintShippingLabelsResult>> Handle(BulkPrintShippingLabelsCommand request, CancellationToken cancellationToken)
    {
        var normalizedOrderIds = request.OrderIds?.Where(id => id != Guid.Empty).ToList() ?? new List<Guid>();
        if (normalizedOrderIds.Count == 0)
        {
            return Result.Failure<BulkPrintShippingLabelsResult>(
                Error.Validation("BulkShippingLabel.NoOrders", "Select at least one order."));
        }

        var uniqueOrderIds = normalizedOrderIds.Distinct().ToList();
        var orders = await _orderRepository.GetByIdsAsync(uniqueOrderIds, cancellationToken);
        var orderLookup = orders.ToDictionary(order => order.Id);

        var items = new List<BulkShippingLabelItemResult>(normalizedOrderIds.Count);
        var seenIds = new HashSet<Guid>();
        var successCount = 0;

        foreach (var orderId in normalizedOrderIds)
        {
            if (!seenIds.Add(orderId))
            {
                items.Add(new BulkShippingLabelItemResult(
                    orderId,
                    false,
                    null,
                    "BulkShippingLabel.DuplicateOrder",
                    "Duplicate order detected in the request."));
                continue;
            }

            if (!orderLookup.TryGetValue(orderId, out var order))
            {
                items.Add(new BulkShippingLabelItemResult(
                    orderId,
                    false,
                    null,
                    "Order.NotFound",
                    "Order not found or unavailable."));
                continue;
            }

            if (order.SellerId != request.SellerId)
            {
                items.Add(new BulkShippingLabelItemResult(
                    orderId,
                    false,
                    null,
                    "Order.AccessDenied",
                    "You do not have permission to update this order."));
                continue;
            }

            var result = await _sender.Send(new PrintShippingLabelCommand(
                orderId,
                request.SellerId,
                request.Carrier,
                request.ServiceCode,
                request.ServiceName,
                request.PackageType,
                request.WeightOz,
                request.LengthIn,
                request.WidthIn,
                request.HeightIn,
                request.ShipDate,
                request.PaymentMethod,
                request.LabelFormat,
                request.LabelPaperSize,
                request.PageWidthIn,
                request.PageHeightIn), cancellationToken);

            if (result.IsFailure)
            {
                items.Add(new BulkShippingLabelItemResult(
                    orderId,
                    false,
                    null,
                    result.Error.Code,
                    result.Error.Description));
                continue;
            }

            successCount++;
            items.Add(new BulkShippingLabelItemResult(
                orderId,
                true,
                result.Value,
                null,
                null));
        }

        var failureCount = items.Count - successCount;
        var payload = new BulkPrintShippingLabelsResult(successCount, failureCount, items);
        return Result.Success(payload);
    }
}
