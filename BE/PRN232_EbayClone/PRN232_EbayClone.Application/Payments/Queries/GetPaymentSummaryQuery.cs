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

public sealed record GetPaymentSummaryQuery(Guid SellerId) : IQuery<PaymentSummaryResponseDto>;

public sealed class GetPaymentSummaryQueryValidator : AbstractValidator<GetPaymentSummaryQuery>
{
    public GetPaymentSummaryQueryValidator()
    {
        RuleFor(x => x.SellerId)
            .NotEmpty().WithMessage("SellerId cannot be empty.");
    }
}

public sealed class GetPaymentSummaryQueryHandler : IQueryHandler<GetPaymentSummaryQuery, PaymentSummaryResponseDto>
{
    private static readonly string[] IncludedBuckets =
    {
        "available",
        "processing",
        "onhold"
    };

    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IShippingLabelRepository _shippingLabelRepository;

    public GetPaymentSummaryQueryHandler(
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IShippingLabelRepository shippingLabelRepository)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _shippingLabelRepository = shippingLabelRepository;
    }

    public async Task<Result<PaymentSummaryResponseDto>> Handle(GetPaymentSummaryQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(new UserId(request.SellerId), cancellationToken);
        if (user is null)
        {
            return Error.Failure("Payments.UserNotFound", "Seller not found.");
        }

        var nowUtc = DateTime.UtcNow;
        var fromUtc = nowUtc.AddDays(-120);

        var orders = await _orderRepository.GetOrdersForSellerAsync(
            request.SellerId,
            fromUtc,
            nowUtc,
            cancellationToken);

        var labelsByOrder = await LoadShippingLabelsAsync(orders, cancellationToken);
        var transactions = PaymentAnalyticsHelper.BuildTransactions(orders, labelsByOrder);

        if (transactions.Count == 0)
        {
            var emptyCurrency = DetermineCurrency(transactions, orders);

            var emptyResponse = new PaymentSummaryResponseDto(
                new PaymentSummaryFundsDto(0m, 0m, 0m, emptyCurrency),
                new PaymentSummaryScheduleDto(
                    BuildAccountDisplay(user),
                    DetermineFrequency(user),
                    null,
                    0m,
                    DetermineNextPayout(null, DetermineFrequency(user)),
                    emptyCurrency),
                Array.Empty<PaymentSummaryActivityItemDto>());

            return Result.Success(emptyResponse);
        }

        var currency = DetermineCurrency(transactions, orders);

        var fundsByBucket = transactions
            .Where(txn => IncludedBuckets.Contains(txn.Bucket, StringComparer.OrdinalIgnoreCase))
            .GroupBy(txn => txn.Bucket, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => PaymentAnalyticsHelper.Round(group.Sum(txn => txn.BalanceImpact > 0 ? txn.BalanceImpact : 0m)),
                StringComparer.OrdinalIgnoreCase);

        fundsByBucket.TryGetValue("available", out var availableAmount);
        fundsByBucket.TryGetValue("processing", out var processingAmount);
        fundsByBucket.TryGetValue("onhold", out var onHoldAmount);

        var lastPayout = transactions
            .Where(txn => string.Equals(txn.Type, "payout", StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(txn => txn.OccurredAt)
            .FirstOrDefault();

        var scheduleFrequency = DetermineFrequency(user);
        var nextPayoutUtc = DetermineNextPayout(lastPayout?.OccurredAt, scheduleFrequency);

        var fundsDto = new PaymentSummaryFundsDto(
            availableAmount,
            processingAmount,
            onHoldAmount,
            currency);

        var scheduleDto = new PaymentSummaryScheduleDto(
            BuildAccountDisplay(user),
            scheduleFrequency,
            lastPayout?.OccurredAt,
            PaymentAnalyticsHelper.Round(Math.Abs(lastPayout?.Amount ?? 0m)),
            nextPayoutUtc,
            currency);

        var recentActivity = transactions
            .OrderByDescending(txn => txn.OccurredAt)
            .Take(8)
            .Select(txn => new PaymentSummaryActivityItemDto(
                txn.Id,
                txn.OccurredAt,
                txn.Type,
                txn.Description,
                txn.Status,
                PaymentAnalyticsHelper.Round(txn.Amount),
                txn.Currency,
                txn.OrderNumber,
                txn.Buyer))
            .ToList();

        var response = new PaymentSummaryResponseDto(fundsDto, scheduleDto, recentActivity);
        return Result.Success(response);
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

    private static string DetermineFrequency(User user)
        => user.IsPaymentVerified ? "Daily" : "Weekly";

    private static DateTime? DetermineNextPayout(DateTime? lastPayoutUtc, string frequency)
    {
        var nowUtc = DateTime.UtcNow;
        var baseline = lastPayoutUtc ?? nowUtc;
        var cadence = string.Equals(frequency, "Daily", StringComparison.OrdinalIgnoreCase) ? 1 : 7;
        var targetDate = baseline.Date.AddDays(cadence).AddHours(9);

        if (targetDate <= nowUtc)
        {
            targetDate = nowUtc.Date.AddDays(1).AddHours(9);
        }

        return targetDate;
    }

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
