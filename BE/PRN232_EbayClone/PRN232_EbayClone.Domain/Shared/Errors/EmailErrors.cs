using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Shared.Errors;

public static class EmailErrors
{
    public readonly static Error Invalid = Error.Failure(
        "Email.Invalid",
        "Email không hợp lệ");
}
