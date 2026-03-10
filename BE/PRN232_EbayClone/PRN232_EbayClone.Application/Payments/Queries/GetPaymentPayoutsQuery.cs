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

public sealed record GetPaymentPayoutsQuery(Guid SellerId, PaymentPayoutsFilterDto Filter) : IQuery<PaymentPayoutsResponseDto>;

public sealed class GetPaymentPayoutsQueryValidator : AbstractValidator<GetPaymentPayoutsQuery>
{
    private static readonly string[] AllowedPeriods =
    {
        "last30Days",
        "last60Days",
        "last90Days",
        "custom"
    };

    private static readonly string[] AllowedSearchBy =
    {
        "payoutId",
        "orderNumber",
        "buyerUsername"
    };

    public GetPaymentPayoutsQueryValidator()
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

            RuleFor(x => x.Filter.SearchBy)
                .NotEmpty()
                .Must(value => AllowedSearchBy.Contains(value.Trim(), StringComparer.OrdinalIgnoreCase))
                .WithMessage("Unsupported search selection.");

            RuleFor(x => x.Filter.Page)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.Filter.PageSize)
                .InclusiveBetween(1, 100);

            When(x => string.Equals(x.Filter.Period, "custom", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.Filter.FromUtc)
                    .NotNull().WithMessage("Custom period requires a start date.");

                RuleFor(x => x.Filter.ToUtc)
                    .NotNull().WithMessage("Custom period requires an end date.");
            });
        });
    }
}

public sealed class GetPaymentPayoutsQueryHandler : IQueryHandler<GetPaymentPayoutsQuery, PaymentPayoutsResponseDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IShippingLabelRepository _shippingLabelRepository;

    public GetPaymentPayoutsQueryHandler(
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IShippingLabelRepository shippingLabelRepository)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _shippingLabelRepository = shippingLabelRepository;
    }

    public async Task<Result<PaymentPayoutsResponseDto>> Handle(GetPaymentPayoutsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(new UserId(request.SellerId), cancellationToken);
        if (user is null)
        {
            return Error.Failure("Payments.UserNotFound", "Seller not found.");
        }

        var filter = request.Filter ?? new PaymentPayoutsFilterDto();

        var (rangeStartUtc, rangeEndUtc) = ResolveRange(filter);
        var fetchStartUtc = rangeStartUtc.AddDays(-7);
        var fetchEndUtc = rangeEndUtc.AddDays(7);

        var orders = await _orderRepository.GetOrdersForSellerAsync(
            request.SellerId,
            fetchStartUtc,
            fetchEndUtc,
            cancellationToken);

        if (orders.Count == 0)
        {
            var emptyResponse = new PaymentPayoutsResponseDto(
                Array.Empty<PaymentPayoutSummaryDto>(),
                0,
                0m,
                "USD");
            return Result.Success(emptyResponse);
        }

        var labelsByOrder = await LoadShippingLabelsAsync(orders, cancellationToken);
        var transactions = PaymentAnalyticsHelper.BuildTransactions(orders, labelsByOrder);
        var aggregations = PaymentPayoutAggregationHelper.BuildAggregations(orders, transactions, rangeStartUtc, rangeEndUtc);

        if (!aggregations.Any())
        {
            var currencyFallback = DetermineCurrency(transactions, orders);
            var emptyResponse = new PaymentPayoutsResponseDto(
                Array.Empty<PaymentPayoutSummaryDto>(),
                0,
                0m,
                currencyFallback);
            return Result.Success(emptyResponse);
        }

        var filteredAggregations = ApplySearchFilter(aggregations, filter.SearchBy, filter.Keyword);

        var currency = DetermineCurrency(transactions, orders);
        var accountDisplay = BuildAccountDisplay(user);

        var totalCount = filteredAggregations.Count;
        var totalAmount = totalCount == 0
            ? 0m
            : PaymentAnalyticsHelper.Round(filteredAggregations.Sum(a => a.TotalAmount));

        var page = filter.Page <= 0 ? 1 : filter.Page;
        var pageSize = filter.PageSize <= 0 ? 20 : Math.Min(filter.PageSize, 100);

        var pagedAggregations = filteredAggregations
            .OrderByDescending(a => a.LatestOccurredAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var payouts = pagedAggregations
            .Select(aggregation => new PaymentPayoutSummaryDto(
                aggregation.PayoutId,
                aggregation.LatestOccurredAtUtc,
                DetermineStatus(aggregation.LatestOccurredAtUtc),
                accountDisplay,
                PaymentPayoutAggregationHelper.BuildMemo(aggregation.TransactionCount),
                aggregation.TransactionCount,
                PaymentAnalyticsHelper.Round(aggregation.TotalAmount),
                currency))
            .ToList();

        var response = new PaymentPayoutsResponseDto(payouts, totalCount, totalAmount, currency);
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

    private static (DateTime StartUtc, DateTime EndUtc) ResolveRange(PaymentPayoutsFilterDto filter)
    {
        var nowUtc = DateTime.UtcNow;
        var todayUtc = new DateTime(nowUtc.Year, nowUtc.Month, nowUtc.Day, 0, 0, 0, DateTimeKind.Utc);
        var normalizedPeriod = (filter.Period ?? "last30Days").Trim().ToLowerInvariant();

        return normalizedPeriod switch
        {
            "last60days" => (todayUtc.AddDays(-59), nowUtc),
            "last90days" => (todayUtc.AddDays(-89), nowUtc),
            "custom" when filter.FromUtc.HasValue && filter.ToUtc.HasValue =>
                NormalizeCustomRange(filter.FromUtc.Value, filter.ToUtc.Value),
            _ => (todayUtc.AddDays(-29), nowUtc)
        };
    }

    private static (DateTime StartUtc, DateTime EndUtc) NormalizeCustomRange(DateTime fromUtc, DateTime toUtc)
    {
        var start = fromUtc <= toUtc ? fromUtc : toUtc;
        var end = fromUtc <= toUtc ? toUtc : fromUtc;
        return (start, end);
    }

    private static List<PaymentPayoutAggregationHelper.PaymentPayoutAggregation> ApplySearchFilter(
        List<PaymentPayoutAggregationHelper.PaymentPayoutAggregation> aggregations,
        string searchBy,
        string? keyword)
    {
        if (aggregations.Count == 0 || string.IsNullOrWhiteSpace(keyword))
        {
            return aggregations;
        }

        var normalizedKeyword = keyword.Trim();
        var mode = (searchBy ?? string.Empty).Trim().ToLowerInvariant();

        return mode switch
        {
            "ordernumber" => aggregations
                .Where(agg => agg.OrderNumbers.Any(orderNumber =>
                    orderNumber.Contains(normalizedKeyword, StringComparison.OrdinalIgnoreCase)))
                .ToList(),
            "buyerusername" => aggregations
                .Where(agg => agg.BuyerUsernames.Any(buyer =>
                    buyer.Contains(normalizedKeyword, StringComparison.OrdinalIgnoreCase)))
                .ToList(),
            "payoutid" => aggregations
                .Where(agg => agg.PayoutId.Contains(normalizedKeyword, StringComparison.OrdinalIgnoreCase))
                .ToList(),
            _ => aggregations
                .Where(agg => agg.PayoutId.Contains(normalizedKeyword, StringComparison.OrdinalIgnoreCase)
                              || agg.OrderNumbers.Any(orderNumber => orderNumber.Contains(normalizedKeyword, StringComparison.OrdinalIgnoreCase))
                              || agg.BuyerUsernames.Any(buyer => buyer.Contains(normalizedKeyword, StringComparison.OrdinalIgnoreCase)))
                .ToList()
        };
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
