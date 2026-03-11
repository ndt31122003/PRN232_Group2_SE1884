using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PRN232_EbayClone.Application.Payments.Dtos;
using PRN232_EbayClone.Domain.Orders.Entities;

namespace PRN232_EbayClone.Application.Payments.Services;

internal static class PaymentPayoutAggregationHelper
{
    internal static List<PaymentPayoutAggregation> BuildAggregations(
        IReadOnlyCollection<Order> orders,
        IReadOnlyCollection<PaymentTransactionDto> transactions,
        DateTime startUtc,
        DateTime endUtc)
    {
        if (orders.Count == 0 || transactions.Count == 0)
        {
            return new List<PaymentPayoutAggregation>();
        }

        var payoutTransactionsByOrder = transactions
            .Where(txn => string.Equals(txn.Type, "payout", StringComparison.OrdinalIgnoreCase)
                           && !string.IsNullOrWhiteSpace(txn.OrderNumber))
            .GroupBy(txn => txn.OrderNumber!, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => group
                    .OrderByDescending(txn => txn.OccurredAt)
                    .First(),
                StringComparer.OrdinalIgnoreCase);

        if (payoutTransactionsByOrder.Count == 0)
        {
            return new List<PaymentPayoutAggregation>();
        }

        var transactionsByOrder = transactions
            .Where(txn => !string.IsNullOrWhiteSpace(txn.OrderNumber))
            .GroupBy(txn => txn.OrderNumber!, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<PaymentTransactionDto>)group
                    .OrderBy(txn => txn.OccurredAt)
                    .ToList(),
                StringComparer.OrdinalIgnoreCase);

        var aggregations = new Dictionary<string, PaymentPayoutAggregation>(StringComparer.OrdinalIgnoreCase);

        foreach (var order in orders)
        {
            if (!payoutTransactionsByOrder.TryGetValue(order.OrderNumber, out var payoutTransaction))
            {
                continue;
            }

            var occurredAt = payoutTransaction.OccurredAt;
            if (occurredAt < startUtc || occurredAt > endUtc)
            {
                continue;
            }

            var payoutId = BuildPayoutId(occurredAt);

            if (!aggregations.TryGetValue(payoutId, out var aggregation))
            {
                aggregation = new PaymentPayoutAggregation(payoutId, occurredAt);
                aggregations.Add(payoutId, aggregation);
            }

            var orderTransactions = transactionsByOrder.TryGetValue(order.OrderNumber, out var related)
                ? related
                : Array.Empty<PaymentTransactionDto>();

            aggregation.Add(order, payoutTransaction, orderTransactions);
        }

        return aggregations.Values
            .OrderByDescending(agg => agg.LatestOccurredAtUtc)
            .ToList();
    }

    internal static string BuildMemo(int transactionCount)
        => transactionCount == 1 ? "1 transaction" : $"{transactionCount} transactions";

    internal static string BuildPayoutId(DateTime occurredAtUtc)
        => $"PY-{occurredAtUtc:yyyyMMdd}";

    internal static bool TryParsePayoutId(string payoutId, out DateTime payoutDateUtc)
    {
        payoutDateUtc = default;

        if (string.IsNullOrWhiteSpace(payoutId))
        {
            return false;
        }

        if (!payoutId.StartsWith("PY-", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var datePart = payoutId[3..];
        if (datePart.Length != 8)
        {
            return false;
        }

        if (!DateTime.TryParseExact(
                datePart,
                "yyyyMMdd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out var parsed))
        {
            return false;
        }

        payoutDateUtc = DateTime.SpecifyKind(parsed, DateTimeKind.Utc);
        return true;
    }

    internal sealed class PaymentPayoutAggregation
    {
        private readonly List<PayoutOrder> _entries = new();

        internal PaymentPayoutAggregation(string payoutId, DateTime occurredAtUtc)
        {
            PayoutId = payoutId;
            FirstOccurredAtUtc = occurredAtUtc;
            LatestOccurredAtUtc = occurredAtUtc;
        }

        internal string PayoutId { get; }
        internal DateTime FirstOccurredAtUtc { get; private set; }
        internal DateTime LatestOccurredAtUtc { get; private set; }
        internal decimal TotalAmount { get; private set; }
        internal IReadOnlyList<PayoutOrder> Entries => _entries;
        internal HashSet<string> OrderNumbers { get; } = new(StringComparer.OrdinalIgnoreCase);
        internal HashSet<string> BuyerUsernames { get; } = new(StringComparer.OrdinalIgnoreCase);
        internal int TransactionCount => _entries.Count;

        internal void Add(Order order, PaymentTransactionDto payoutTransaction, IReadOnlyList<PaymentTransactionDto> transactions)
        {
            _entries.Add(new PayoutOrder(order, payoutTransaction, transactions));
            OrderNumbers.Add(order.OrderNumber);

            if (!string.IsNullOrWhiteSpace(order.Buyer?.Username))
            {
                BuyerUsernames.Add(order.Buyer.Username);
            }

            TotalAmount += Math.Abs(payoutTransaction.Amount);

            if (payoutTransaction.OccurredAt < FirstOccurredAtUtc)
            {
                FirstOccurredAtUtc = payoutTransaction.OccurredAt;
            }

            if (payoutTransaction.OccurredAt > LatestOccurredAtUtc)
            {
                LatestOccurredAtUtc = payoutTransaction.OccurredAt;
            }
        }

        internal sealed record PayoutOrder(
            Order Order,
            PaymentTransactionDto PayoutTransaction,
            IReadOnlyList<PaymentTransactionDto> Transactions);
    }
}
