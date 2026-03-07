using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Vouchers.Enums;

namespace PRN232_EbayClone.Domain.Vouchers.Entities;

public sealed class VoucherTransaction : AggregateRoot<Guid>
{
    public Guid VoucherId { get; private set; }
    public Voucher? Voucher { get; private set; }
    public Guid OrderId { get; private set; }
    public decimal AmountUsed { get; private set; }
    public VoucherTransactionType TransactionType { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public string? Notes { get; private set; }

    private VoucherTransaction(Guid id) : base(id)
    {
    }

    public static Result<VoucherTransaction> Create(
        Guid voucherId,
        Guid orderId,
        decimal amountUsed,
        VoucherTransactionType transactionType,
        string? notes = null)
    {
        var transaction = new VoucherTransaction(Guid.NewGuid())
        {
            VoucherId = voucherId,
            OrderId = orderId,
            AmountUsed = amountUsed,
            TransactionType = transactionType,
            TransactionDate = DateTime.UtcNow,
            Notes = notes?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        return Validate(transaction);
    }

    private static Result<VoucherTransaction> Validate(VoucherTransaction transaction)
    {
        if (transaction.VoucherId == Guid.Empty)
        {
            return new Error("VoucherTransaction.InvalidVoucherId", "Voucher ID is required.");
        }

        if (transaction.OrderId == Guid.Empty)
        {
            return new Error("VoucherTransaction.InvalidOrderId", "Order ID is required.");
        }

        if (transaction.AmountUsed <= 0)
        {
            return new Error("VoucherTransaction.InvalidAmount", "Amount must be greater than 0.");
        }

        return Result.Success(transaction);
    }
}
