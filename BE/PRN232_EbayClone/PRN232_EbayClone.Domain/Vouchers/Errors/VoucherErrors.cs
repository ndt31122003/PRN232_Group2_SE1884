using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Vouchers.Errors;

public static class VoucherErrors
{
    public static Error EmptyCode => new("Voucher.EmptyCode", "Voucher code cannot be empty.");
    public static Error InvalidCodeFormat => new("Voucher.InvalidCodeFormat", "Voucher code can only contain letters, numbers, and hyphens.");
    public static Error InvalidInitialValue => new("Voucher.InvalidInitialValue", "Initial value must be greater than 0.");
    public static Error InvalidExpiryDate => new("Voucher.InvalidExpiryDate", "Expiry date must be after issue date.");
    public static Error InvalidTopUpAmount => new("Voucher.InvalidTopUpAmount", "Top-up amount must be greater than 0.");
    public static Error InsufficientBalance => new("Voucher.InsufficientBalance", "Voucher has insufficient balance.");
    public static Error ExpiredVoucher => new("Voucher.ExpiredVoucher", "Voucher has expired.");
    public static Error InactiveVoucher => new("Voucher.InactiveVoucher", "Voucher is not active.");
    public static Error NotTransferable => new("Voucher.NotTransferable", "Voucher is not transferable.");
    public static Error AmountExceedsBalance => new("Voucher.AmountExceedsBalance", "Amount exceeds voucher balance.");
    public static Error RefundExceedsInitialValue => new("Voucher.RefundExceedsInitialValue", "Refund cannot exceed initial voucher value.");
    public static Error DuplicateCode => new("Voucher.DuplicateCode", "Voucher code already exists.");
}
