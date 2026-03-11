using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Payments.Dtos;
using PRN232_EbayClone.Application.Payments.Services;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Payments.Queries;

public sealed record GetPaymentReportQuery(Guid SellerId, PaymentReportFilterDto Filter) : IQuery<PaymentReportResponseDto>;

public sealed class GetPaymentReportQueryValidator : AbstractValidator<GetPaymentReportQuery>
{
    private static readonly string[] AllowedPeriods =
    {
        "thisMonth",
        "lastMonth",
        "last90Days",
        "thisYear"
    };

    private static readonly string[] AllowedComparedValues =
    {
        "none",
        "previousPeriod",
        "samePeriodLastYear"
    };

    public GetPaymentReportQueryValidator()
    {
        RuleFor(x => x.SellerId)
            .NotEmpty().WithMessage("SellerId cannot be empty.");

        RuleFor(x => x.Filter)
            .NotNull();

        When(x => x.Filter is not null, () =>
        {
            RuleFor(x => x.Filter.Period)
                .NotEmpty()
                .Must(value => AllowedPeriods.Contains(value.Trim(), StringComparer.OrdinalIgnoreCase))
                .WithMessage("Unsupported period selection.");

            RuleFor(x => x.Filter.Compared)
                .NotEmpty()
                .Must(value => AllowedComparedValues.Contains(value.Trim(), StringComparer.OrdinalIgnoreCase))
                .WithMessage("Unsupported comparison selection.");
        });
    }
}

public sealed class GetPaymentReportQueryHandler : IQueryHandler<GetPaymentReportQuery, PaymentReportResponseDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IShippingLabelRepository _shippingLabelRepository;

    public GetPaymentReportQueryHandler(
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IShippingLabelRepository shippingLabelRepository)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _shippingLabelRepository = shippingLabelRepository;
    }

    public async Task<Result<PaymentReportResponseDto>> Handle(GetPaymentReportQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(new UserId(request.SellerId), cancellationToken);
        if (user is null)
        {
            return Error.Failure("Payments.UserNotFound", "Seller not found.");
        }

        var (currentRangeStart, currentRangeEnd) = ResolvePeriodRange(request.Filter.Period);

        var currentOrders = await _orderRepository.GetOrdersForSellerAsync(
            request.SellerId,
            currentRangeStart,
            currentRangeEnd,
            cancellationToken);

        var currentLabels = await LoadShippingLabelsAsync(currentOrders, cancellationToken);
        var currentTransactions = PaymentAnalyticsHelper.BuildTransactions(currentOrders, currentLabels);
        currentTransactions = currentTransactions
            .Where(txn => txn.OccurredAt >= currentRangeStart && txn.OccurredAt <= currentRangeEnd)
            .ToList();

        var currency = DetermineCurrency(currentTransactions, currentOrders);
        var currentPeriod = PaymentAnalyticsHelper.BuildReportPeriod(
            currentOrders,
            currentTransactions,
            currentLabels,
            currentRangeStart,
            currentRangeEnd,
            currency);

        PaymentReportPeriodDto? comparisonPeriod = null;

        if (!string.Equals(request.Filter.Compared, "none", StringComparison.OrdinalIgnoreCase))
        {
            var comparisonRange = ResolveComparisonRange(request.Filter.Compared, currentRangeStart, currentRangeEnd);
            if (comparisonRange is not null)
            {
                var (comparisonStart, comparisonEnd) = comparisonRange.Value;

                var comparisonOrders = await _orderRepository.GetOrdersForSellerAsync(
                    request.SellerId,
                    comparisonStart,
                    comparisonEnd,
                    cancellationToken);

                var comparisonLabels = await LoadShippingLabelsAsync(comparisonOrders, cancellationToken);
                var comparisonTransactions = PaymentAnalyticsHelper.BuildTransactions(comparisonOrders, comparisonLabels)
                    .Where(txn => txn.OccurredAt >= comparisonStart && txn.OccurredAt <= comparisonEnd)
                    .ToList();

                var comparisonCurrency = DetermineCurrency(comparisonTransactions, comparisonOrders) ?? currency;

                comparisonPeriod = PaymentAnalyticsHelper.BuildReportPeriod(
                    comparisonOrders,
                    comparisonTransactions,
                    comparisonLabels,
                    comparisonStart,
                    comparisonEnd,
                    comparisonCurrency);
            }
        }

        var response = new PaymentReportResponseDto(currentPeriod, comparisonPeriod);
        return Result.Success(response);
    }

    private static (DateTime StartUtc, DateTime EndUtc) ResolvePeriodRange(string period)
    {
        var now = DateTime.UtcNow;
        var utcToday = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);

        return period.ToLowerInvariant() switch
        {
            "thismonth" =>
                (new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc),
                 new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1).AddTicks(-1)),
            "lastmonth" =>
                (new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(-1),
                 new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddTicks(-1)),
            "last90days" =>
                (utcToday.AddDays(-89), utcToday.AddDays(1).AddTicks(-1)),
            "thisyear" =>
                (new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                 new DateTime(now.Year + 1, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddTicks(-1)),
            _ =>
                (utcToday.AddDays(-89), utcToday.AddDays(1).AddTicks(-1))
        };
    }

    private static (DateTime StartUtc, DateTime EndUtc)? ResolveComparisonRange(string compared, DateTime currentStart, DateTime currentEnd)
    {
        var span = currentEnd - currentStart;

        return compared.ToLowerInvariant() switch
        {
            "previousperiod" => (currentStart.AddTicks(-(span.Ticks + 1)), currentStart.AddTicks(-1)),
            "sameperiodlastyear" => (currentStart.AddYears(-1), currentEnd.AddYears(-1)),
            _ => null
        };
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
}
