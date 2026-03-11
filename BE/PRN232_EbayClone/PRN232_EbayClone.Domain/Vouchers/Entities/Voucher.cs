using System.Text.RegularExpressions;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Vouchers.Errors;

namespace PRN232_EbayClone.Domain.Vouchers.Entities;

public sealed class Voucher : AggregateRoot<Guid>
{
    public Guid? SellerId { get; private set; }
    public string Code { get; private set; } = null!;
    public decimal InitialValue { get; private set; }
    public decimal CurrentBalance { get; private set; }
    public string Currency { get; private set; } = "USD";
    public DateTime IssueDate { get; private set; }
    public DateTime? ExpiryDate { get; private set; }
    public Guid? AssignedUserId { get; private set; }
    public bool IsTransferable { get; private set; }
    public bool IsActive { get; private set; }

    private Voucher(Guid id) : base(id)
    {
    }

    public static Result<Voucher> Create(
        Guid? sellerId,
        string code,
        decimal initialValue,
        string currency,
        DateTime issueDate,
        DateTime? expiryDate,
        Guid? assignedUserId,
        bool isTransferable,
        bool isActive)
    {
        var voucher = new Voucher(Guid.NewGuid())
        {
            SellerId = sellerId,
            Code = code.Trim().ToUpperInvariant(),
            InitialValue = initialValue,
            CurrentBalance = initialValue,
            Currency = currency?.Trim().ToUpperInvariant() ?? "USD",
            IssueDate = issueDate,
            ExpiryDate = expiryDate,
            AssignedUserId = assignedUserId,
            IsTransferable = isTransferable,
            IsActive = isActive,
            CreatedAt = DateTime.UtcNow
        };

        return Validate(voucher);
    }

    public Result Use(decimal amount, Guid orderId)
    {
        if (!IsActive)
        {
            return VoucherErrors.InactiveVoucher;
        }

        if (CurrentBalance <= 0)
        {
            return VoucherErrors.InsufficientBalance;
        }

        if (ExpiryDate.HasValue && ExpiryDate.Value < DateTime.UtcNow)
        {
            return VoucherErrors.ExpiredVoucher;
        }

        if (!IsTransferable && AssignedUserId.HasValue && AssignedUserId != SellerId)
        {
            return VoucherErrors.NotTransferable;
        }

        if (amount > CurrentBalance)
        {
            return VoucherErrors.AmountExceedsBalance;
        }

        CurrentBalance -= amount;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result Refund(decimal amount)
    {
        if (amount > InitialValue)
        {
            return VoucherErrors.RefundExceedsInitialValue;
        }

        if (CurrentBalance + amount > InitialValue)
        {
            return VoucherErrors.RefundExceedsInitialValue;
        }

        CurrentBalance += amount;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result TopUp(decimal amount)
    {
        if (amount <= 0)
        {
            return VoucherErrors.InvalidTopUpAmount;
        }

        InitialValue += amount;
        CurrentBalance += amount;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result AssignToUser(Guid userId)
    {
        if (!IsTransferable && AssignedUserId.HasValue)
        {
            return VoucherErrors.NotTransferable;
        }

        AssignedUserId = userId;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    private static Result<Voucher> Validate(Voucher voucher)
    {
        if (string.IsNullOrWhiteSpace(voucher.Code))
        {
            return VoucherErrors.EmptyCode;
        }

        if (!Regex.IsMatch(voucher.Code, "^[A-Z0-9-]+$"))
        {
            return VoucherErrors.InvalidCodeFormat;
        }

        if (voucher.InitialValue <= 0)
        {
            return VoucherErrors.InvalidInitialValue;
        }

        if (voucher.ExpiryDate.HasValue && voucher.ExpiryDate <= voucher.IssueDate)
        {
            return VoucherErrors.InvalidExpiryDate;
        }

        return Result.Success(voucher);
    }
}
