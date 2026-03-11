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
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Payments.Queries;

public sealed record GetPaymentTransactionsQuery(
    Guid SellerId,
    PaymentTransactionsFilterDto Filter) : IQuery<PaymentTransactionsResponseDto>;

public sealed class GetPaymentTransactionsQueryValidator : AbstractValidator<GetPaymentTransactionsQuery>
{
    private static readonly HashSet<string> AllowedStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        "all", "available", "processing", "onhold", "payout", "charge", "repayment"
    };

    private static readonly HashSet<string> AllowedTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "all", "sale", "shippinglabel", "fee", "claim", "payout", "adjustment", "refund"
    };

    private static readonly HashSet<string> AllowedSearchFields = new(StringComparer.OrdinalIgnoreCase)
    {
        "ordernumber", "buyer", "itemid", "caseid", "trackingnumber"
    };

    private static readonly int[] AllowedPeriods = { 30, 60, 90 };

    public GetPaymentTransactionsQueryValidator()
    {
        RuleFor(x => x.SellerId)
            .NotEmpty().WithMessage("SellerId cannot be empty.");

        RuleFor(x => x.Filter)
            .NotNull();

        When(x => x.Filter is not null, () =>
        {
            RuleFor(x => x.Filter.Status)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(status => AllowedStatuses.Contains(status.Trim()))
                .WithMessage("Invalid status filter.");

            RuleFor(x => x.Filter.Type)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(type => AllowedTypes.Contains(type.Trim()))
                .WithMessage("Invalid transaction type.");

            RuleFor(x => x.Filter.PeriodDays)
                .Must(period => AllowedPeriods.Contains(period))
                .WithMessage("PeriodDays must be one of: 30, 60, 90.");

            RuleFor(x => x.Filter.SearchField)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(field => AllowedSearchFields.Contains(field.Trim()))
                .WithMessage("Invalid search field.");
        });
    }
}

public sealed class GetPaymentTransactionsQueryHandler : IQueryHandler<GetPaymentTransactionsQuery, PaymentTransactionsResponseDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IShippingLabelRepository _shippingLabelRepository;

    public GetPaymentTransactionsQueryHandler(
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IShippingLabelRepository shippingLabelRepository)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _shippingLabelRepository = shippingLabelRepository;
    }

    public async Task<Result<PaymentTransactionsResponseDto>> Handle(GetPaymentTransactionsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(new UserId(request.SellerId), cancellationToken);
        if (user is null)
        {
            return Error.Failure("Payments.UserNotFound", "Seller not found.");
        }

        var normalizedFilter = NormalizeFilter(request.Filter);

        var orders = await _orderRepository.GetOrdersForSellerAsync(
            request.SellerId,
            normalizedFilter.FromUtc,
            normalizedFilter.ToUtc,
            cancellationToken);

        var labelsByOrder = await LoadShippingLabelsAsync(orders, cancellationToken);
        var allTransactions = PaymentAnalyticsHelper.BuildTransactions(orders, labelsByOrder);
        var periodTransactions = allTransactions
            .Where(t => t.OccurredAt >= normalizedFilter.FromUtc && t.OccurredAt <= normalizedFilter.ToUtc)
            .ToList();

        var summaryCounts = periodTransactions
            .GroupBy(t => t.Bucket, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => group.Count(),
                StringComparer.OrdinalIgnoreCase);

        foreach (var bucket in PaymentAnalyticsHelper.Buckets)
        {
            if (!summaryCounts.ContainsKey(bucket))
            {
                summaryCounts[bucket] = 0;
            }
        }

        var currentBalance = periodTransactions.Sum(t => t.BalanceImpact);
        var currency = periodTransactions.FirstOrDefault()?.Currency
                       ?? orders.FirstOrDefault()?.Total.Currency
                       ?? "USD";

        var filteredTransactions = ApplyFilters(periodTransactions, normalizedFilter);

        var orderedTransactions = filteredTransactions
            .OrderByDescending(t => t.OccurredAt)
            .ToList();

        return Result.Success(new PaymentTransactionsResponseDto(
            orderedTransactions,
            currentBalance,
            currency,
            summaryCounts));
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

    private static List<PaymentTransactionDto> ApplyFilters(
        List<PaymentTransactionDto> transactions,
        NormalizedFilter filter)
    {
        var filtered = transactions.AsEnumerable();

        if (!string.Equals(filter.Status, "all", StringComparison.OrdinalIgnoreCase))
        {
            filtered = filtered.Where(t => string.Equals(t.Bucket, filter.Status, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.Equals(filter.Type, "all", StringComparison.OrdinalIgnoreCase))
        {
            filtered = filtered.Where(t => string.Equals(t.Type, filter.Type, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            filtered = filtered.Where(t => MatchesSearch(t, filter.SearchField, filter.Search));
        }

        return filtered.ToList();
    }

    private static bool MatchesSearch(PaymentTransactionDto transaction, string field, string query)
    {
        string? candidate = field switch
        {
            "ordernumber" => transaction.OrderNumber,
            "buyer" => transaction.Buyer,
            "itemid" => transaction.ItemId,
            "caseid" => transaction.CaseId,
            "trackingnumber" => transaction.TrackingNumber,
            _ => transaction.OrderNumber
        };

        if (string.IsNullOrWhiteSpace(candidate))
        {
            return false;
        }

        return candidate.Contains(query, StringComparison.OrdinalIgnoreCase);
    }

    private static NormalizedFilter NormalizeFilter(PaymentTransactionsFilterDto filter)
    {
        var status = (filter.Status ?? "all").Trim().ToLowerInvariant();
        var type = (filter.Type ?? "all").Trim().ToLowerInvariant();
        var searchField = (filter.SearchField ?? "ordernumber").Trim().ToLowerInvariant();
        var search = filter.Search?.Trim();
        var period = filter.PeriodDays is 30 or 60 or 90 ? filter.PeriodDays : 90;

        var toUtc = DateTime.UtcNow;
        var fromUtc = toUtc.AddDays(-period);

        return new NormalizedFilter(status, type, period, searchField, search, fromUtc, toUtc);
    }

    private sealed record NormalizedFilter(
        string Status,
        string Type,
        int PeriodDays,
        string SearchField,
        string? Search,
        DateTime FromUtc,
        DateTime ToUtc);
}
