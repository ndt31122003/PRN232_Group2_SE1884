using System;
using System.Collections.Generic;
using System.Linq;
using PRN232_EbayClone.Application.Payments.Dtos;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Orders.Entities;

namespace PRN232_EbayClone.Application.Payments.Services;

internal static class PaymentAnalyticsHelper
{
    internal static readonly string[] CompletedStatuses =
    {
        OrderStatusCodes.PaidAndShipped,
        OrderStatusCodes.ShippedAwaitingFeedback,
        OrderStatusCodes.PaidAwaitingFeedback
    };

    internal static readonly string[] Buckets =
    {
        "available",
        "processing",
        "onhold",
        "payout",
        "charge",
        "repayment"
    };

    internal static List<PaymentTransactionDto> BuildTransactions(
        IEnumerable<Order> orders,
        IReadOnlyDictionary<Guid, IReadOnlyList<ShippingLabel>> labelsByOrder)
    {
        var transactions = new List<PaymentTransactionDto>();

        foreach (var order in orders)
        {
            var primaryItem = order.Items.FirstOrDefault();
            var latestLabel = labelsByOrder.TryGetValue(order.Id, out var labels)
                ? labels
                    .OrderByDescending(label => label.PurchasedAt)
                    .FirstOrDefault()
                : null;

            var currency = order.Total.Currency;
            var buyer = order.Buyer?.Username;
            var bucket = ResolveBucket(order.Status.Code);
            var saleStatus = bucket switch
            {
                "available" => "Completed",
                "processing" => "Processing",
                "onhold" => "On hold",
                _ => "Completed"
            };

            var saleAmount = Round(order.Total.Amount);

            transactions.Add(new PaymentTransactionDto(
                Guid.NewGuid(),
                order.OrderedAt,
                "sale",
                primaryItem?.Title ?? "Order sale",
                order.OrderNumber,
                saleStatus,
                bucket,
                currency,
                saleAmount,
                saleAmount,
                buyer,
                primaryItem?.ListingId.ToString(),
                null,
                latestLabel?.TrackingNumber));

            if (order.ShippingCost.Amount > 0)
            {
                var shippingAmount = -Round(order.ShippingCost.Amount);
                var labelDate = latestLabel?.PurchasedAt.UtcDateTime ?? order.OrderedAt.AddHours(6);
                var tracking = latestLabel?.TrackingNumber ?? $"TRK-{order.OrderNumber}";

                transactions.Add(new PaymentTransactionDto(
                    Guid.NewGuid(),
                    labelDate,
                    "shippingLabel",
                    $"Shipping label for {order.OrderNumber}",
                    order.OrderNumber,
                    "Completed",
                    "charge",
                    currency,
                    shippingAmount,
                    shippingAmount,
                    buyer,
                    primaryItem?.ListingId.ToString(),
                    null,
                    tracking));
            }

            if (order.PlatformFee.Amount > 0)
            {
                var feeAmount = -Round(order.PlatformFee.Amount);

                transactions.Add(new PaymentTransactionDto(
                    Guid.NewGuid(),
                    order.PaidAt ?? order.OrderedAt.AddHours(2),
                    "fee",
                    "Final value fee",
                    order.OrderNumber,
                    "Completed",
                    "charge",
                    currency,
                    feeAmount,
                    feeAmount,
                    buyer,
                    primaryItem?.ListingId.ToString(),
                    null,
                    null));
            }

            if (IsCompleted(order.Status.Code))
            {
                var net = saleAmount +
                          (order.ShippingCost.Amount > 0 ? -Round(order.ShippingCost.Amount) : 0) +
                          (order.PlatformFee.Amount > 0 ? -Round(order.PlatformFee.Amount) : 0);

                if (net > 0)
                {
                    var payoutAmount = -Round(net);
                    var payoutDate = (order.PaidAt ?? order.OrderedAt).AddDays(2);

                    transactions.Add(new PaymentTransactionDto(
                        Guid.NewGuid(),
                        payoutDate,
                        "payout",
                        $"Payout for {order.OrderNumber}",
                        order.OrderNumber,
                        "Released",
                        "payout",
                        currency,
                        payoutAmount,
                        payoutAmount,
                        null,
                        null,
                        null,
                        null));
                }
            }
        }

        return transactions;
    }

    internal static PaymentReportPeriodDto BuildReportPeriod(
        IEnumerable<Order> orders,
        IEnumerable<PaymentTransactionDto> transactions,
        IReadOnlyDictionary<Guid, IReadOnlyList<ShippingLabel>> labelsByOrder,
        DateTime startUtc,
        DateTime endUtc,
        string currency)
    {
        var periodOrders = orders
            .Where(order => order.OrderedAt >= startUtc && order.OrderedAt <= endUtc)
            .ToList();

        var periodTransactions = transactions
            .Where(txn => txn.OccurredAt >= startUtc && txn.OccurredAt <= endUtc)
            .ToList();

        var orderProceeds = BuildOrderProceeds(periodOrders, currency);
        var refunds = BuildRefunds(periodTransactions, currency);
        var expenses = BuildExpenses(periodOrders, labelsByOrder, periodTransactions, currency);
        var netTransfers = BuildNetTransfers(expenses, periodTransactions, currency);
        var other = BuildOther(periodTransactions, currency);

        var sections = new Dictionary<string, PaymentReportSectionDto>(StringComparer.OrdinalIgnoreCase)
        {
            ["orderProceeds"] = orderProceeds,
            ["refunds"] = refunds,
            ["expenses"] = expenses,
            ["netTransfers"] = netTransfers,
            ["other"] = other
        };

        return new PaymentReportPeriodDto(
            startUtc,
            endUtc,
            currency,
            sections);
    }

    private static PaymentReportSectionDto BuildOrderProceeds(IEnumerable<Order> orders, string currency)
    {
        var subTotal = orders.Sum(order => order.SubTotal.Amount);
        var discount = orders.Sum(order => order.DiscountAmount.Amount);
        var tax = orders.Sum(order => order.TaxAmount.Amount);
        var shipping = orders.Sum(order => order.ShippingCost.Amount);
        var total = orders.Sum(order => order.Total.Amount);

        var lines = new List<PaymentReportLineDto>
        {
            new("Item subtotal", Round(subTotal)),
            new("Discount", Round(discount)),
            new("Seller collected tax", Round(tax)),
            new("Shipping and handling", Round(shipping))
        };

        return new PaymentReportSectionDto("Order proceeds", Round(total), lines);
    }

    private static PaymentReportSectionDto BuildRefunds(IEnumerable<PaymentTransactionDto> transactions, string currency)
    {
        var refunds = transactions
            .Where(txn => string.Equals(txn.Type, "refund", StringComparison.OrdinalIgnoreCase))
            .ToList();

        var grossRefunds = Round(refunds.Sum(txn => Math.Abs(txn.Amount)));

        var lines = new List<PaymentReportLineDto>
        {
            new("Gross refunds", grossRefunds),
            new("Gross claims", 0m),
            new("Gross payment disputes", 0m)
        };

        return new PaymentReportSectionDto("Refunds", grossRefunds, lines);
    }

    private static PaymentReportSectionDto BuildExpenses(
        IEnumerable<Order> orders,
        IReadOnlyDictionary<Guid, IReadOnlyList<ShippingLabel>> labelsByOrder,
        IEnumerable<PaymentTransactionDto> transactions,
        string currency)
    {
        var fees = orders.Sum(order => order.PlatformFee.Amount);
        var shippingLabelCost = orders.Sum(order =>
        {
            if (!labelsByOrder.TryGetValue(order.Id, out var labels) || labels.Count == 0)
            {
                return 0m;
            }

            return labels.Sum(label => label.Cost.Amount);
        });

        var donations = 0m; // Placeholder until donations are tracked

        var lines = new List<PaymentReportLineDto>
        {
            new("Fees", Round(fees)),
            new("Shipping labels", Round(shippingLabelCost)),
            new("Donations", donations)
        };

        var total = Round(fees + shippingLabelCost + donations);
        return new PaymentReportSectionDto("Expenses", total, lines);
    }

    private static PaymentReportSectionDto BuildNetTransfers(
        PaymentReportSectionDto expenses,
        IEnumerable<PaymentTransactionDto> transactions,
        string currency)
    {
        var charges = transactions
            .Where(txn => string.Equals(txn.Type, "fee", StringComparison.OrdinalIgnoreCase)
                       || string.Equals(txn.Type, "shippingLabel", StringComparison.OrdinalIgnoreCase)
                       || string.Equals(txn.Type, "charge", StringComparison.OrdinalIgnoreCase))
            .Sum(txn => Math.Abs(txn.Amount));

        var payouts = transactions
            .Where(txn => string.Equals(txn.Type, "payout", StringComparison.OrdinalIgnoreCase))
            .Sum(txn => Math.Abs(txn.Amount));

        var total = Round(payouts - charges);

        var lines = new List<PaymentReportLineDto>
        {
            new("Charges", Round(charges)),
            new("Payouts", Round(payouts))
        };

        return new PaymentReportSectionDto("Net transfers", total, lines);
    }

    private static PaymentReportSectionDto BuildOther(IEnumerable<PaymentTransactionDto> transactions, string currency)
    {
        var otherTransactions = transactions
            .Where(txn => txn.Type is not "sale"
                                     and not "fee"
                                     and not "shippingLabel"
                                     and not "payout"
                                     and not "refund")
            .ToList();

        var total = Round(otherTransactions.Sum(txn => txn.Amount));

        var lines = new List<PaymentReportLineDto>
        {
            new("Adjustments", total)
        };

        return new PaymentReportSectionDto("Other", total, lines);
    }

    internal static bool IsCompleted(string statusCode)
        => CompletedStatuses.Contains(statusCode, StringComparer.OrdinalIgnoreCase);

    internal static string ResolveBucket(string statusCode)
    {
        if (IsCompleted(statusCode))
        {
            return "available";
        }

        return statusCode.Equals(OrderStatusCodes.Cancelled, StringComparison.OrdinalIgnoreCase)
            ? "onhold"
            : "processing";
    }

    internal static decimal Round(decimal value)
        => decimal.Round(value, 2, MidpointRounding.AwayFromZero);
}
