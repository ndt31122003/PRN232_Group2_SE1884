using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Payments.Dtos;
using PRN232_EbayClone.Application.Payments.Services;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Users.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Payments.Queries;

public sealed record GetPaymentPayoutDetailQuery(Guid SellerId, string PayoutId) : IQuery<PaymentPayoutDetailDto>;

public sealed class GetPaymentPayoutDetailQueryValidator : AbstractValidator<GetPaymentPayoutDetailQuery>
{
    public GetPaymentPayoutDetailQueryValidator()
    {
        RuleFor(x => x.SellerId)
            .NotEmpty().WithMessage("SellerId cannot be empty.");

        RuleFor(x => x.PayoutId)
            .NotEmpty().WithMessage("PayoutId cannot be empty.");
    }
}

public sealed class GetPaymentPayoutDetailQueryHandler : IQueryHandler<GetPaymentPayoutDetailQuery, PaymentPayoutDetailDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IShippingLabelRepository _shippingLabelRepository;

    public GetPaymentPayoutDetailQueryHandler(
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IShippingLabelRepository shippingLabelRepository)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _shippingLabelRepository = shippingLabelRepository;
    }

    public async Task<Result<PaymentPayoutDetailDto>> Handle(GetPaymentPayoutDetailQuery request, CancellationToken cancellationToken)
    {
        if (!PaymentPayoutAggregationHelper.TryParsePayoutId(request.PayoutId, out var payoutDateUtc))
        {
            return Error.Failure("Payments.InvalidPayoutId", "Payout identifier is invalid.");
        }

        var user = await _userRepository.GetByIdAsync(new UserId(request.SellerId), cancellationToken);
        if (user is null)
        {
            return Error.Failure("Payments.UserNotFound", "Seller not found.");
        }

        var rangeStartUtc = payoutDateUtc;
        var rangeEndUtc = payoutDateUtc.AddDays(1).AddTicks(-1);
        var fetchStartUtc = rangeStartUtc.AddDays(-2);
        var fetchEndUtc = rangeEndUtc.AddDays(2);

        var orders = await _orderRepository.GetOrdersForSellerAsync(
            request.SellerId,
            fetchStartUtc,
            fetchEndUtc,
            cancellationToken);

        if (orders.Count == 0)
        {
            return Error.Failure("Payments.PayoutNotFound", "Payout not found.");
        }

        var labelsByOrder = await LoadShippingLabelsAsync(orders, cancellationToken);
        var transactions = PaymentAnalyticsHelper.BuildTransactions(orders, labelsByOrder);
        var aggregations = PaymentPayoutAggregationHelper.BuildAggregations(orders, transactions, rangeStartUtc, rangeEndUtc);
        var target = aggregations.FirstOrDefault(agg =>
            string.Equals(agg.PayoutId, request.PayoutId, StringComparison.OrdinalIgnoreCase));

        if (target is null)
        {
            return Error.Failure("Payments.PayoutNotFound", "Payout not found.");
        }

        var currency = DetermineCurrency(transactions, orders);
        var accountDisplay = BuildAccountDisplay(user);
        var status = DetermineStatus(target.LatestOccurredAtUtc);
        var memo = PaymentPayoutAggregationHelper.BuildMemo(target.TransactionCount);

        var transactionDtos = target.Entries
            .Select(entry => MapTransaction(entry, currency))
            .ToList();

        var detail = new PaymentPayoutDetailDto(
            target.PayoutId,
            target.LatestOccurredAtUtc,
            status,
            accountDisplay,
            memo,
            PaymentAnalyticsHelper.Round(target.TotalAmount),
            currency,
            target.TransactionCount,
            transactionDtos);

        return Result.Success(detail);
    }

    private async Task<IReadOnlyDictionary<Guid, IReadOnlyList<ShippingLabel>>> LoadShippingLabelsAsync(
        IReadOnlyCollection<Order> orders,
        CancellationToken cancellationToken)
    {
        if (orders.Count == 0)
        {
            return new Dictionary<Guid, IReadOnlyList<ShippingLabel>>();
        }

        var orderIds = orders
            .Select(order => order.Id)
            .Distinct()
            .ToList();

        var labels = await _shippingLabelRepository.GetByOrderIdsAsync(orderIds, cancellationToken);

        return labels
            .GroupBy(label => label.OrderId)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<ShippingLabel>)group
                    .OrderByDescending(label => label.PurchasedAt)
                    .ToList());
    }

    private static PaymentPayoutTransactionDto MapTransaction(
        PaymentPayoutAggregationHelper.PaymentPayoutAggregation.PayoutOrder entry,
        string currency)
    {
        var order = entry.Order;
        var transactions = entry.Transactions;

        decimal grossAmount = transactions
            .Where(txn => string.Equals(txn.Type, "sale", StringComparison.OrdinalIgnoreCase))
            .Sum(txn => Math.Abs(txn.Amount));

        if (grossAmount <= 0)
        {
            grossAmount = Math.Abs(order.Total.Amount);
        }

        decimal totalFees = transactions
            .Where(txn => string.Equals(txn.Type, "fee", StringComparison.OrdinalIgnoreCase)
                          || string.Equals(txn.Type, "shippingLabel", StringComparison.OrdinalIgnoreCase)
                          || string.Equals(txn.Type, "charge", StringComparison.OrdinalIgnoreCase)
                          || string.Equals(txn.Type, "refund", StringComparison.OrdinalIgnoreCase))
            .Sum(txn => Math.Abs(txn.Amount));

        var netAmount = Math.Abs(entry.PayoutTransaction.Amount);
        var status = order.Status?.Name ?? entry.PayoutTransaction.Status;

        return new PaymentPayoutTransactionDto(
            order.Id,
            order.OrderNumber,
            order.Buyer?.Username,
            status,
            order.OrderedAt,
            order.PaidAt,
            PaymentAnalyticsHelper.Round(grossAmount),
            PaymentAnalyticsHelper.Round(totalFees),
            PaymentAnalyticsHelper.Round(netAmount),
            currency);
    }

    private static string DetermineStatus(DateTime payoutDateUtc)
        => payoutDateUtc <= DateTime.UtcNow ? "Completed" : "Processing";

    private static string DetermineCurrency(
        IReadOnlyCollection<PaymentTransactionDto> transactions,
        IReadOnlyCollection<Order> orders)
    {
        var currency = transactions.FirstOrDefault()?.Currency;
        if (!string.IsNullOrWhiteSpace(currency))
        {
            return currency;
        }

        var orderCurrency = orders.FirstOrDefault()?.Total.Currency;
        return string.IsNullOrWhiteSpace(orderCurrency) ? "USD" : orderCurrency;
    }

    private static string BuildAccountDisplay(User user)
    {
        var seed = Math.Abs(user.Id.Value.GetHashCode());
        var lastFour = (seed % 9000) + 1000;
        return $"Checking account ••••{lastFour:D4}";
    }
}
