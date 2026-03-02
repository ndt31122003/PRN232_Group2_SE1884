namespace PRN232_EbayClone.Domain.Shared.Results;

public sealed record ValidationError : Error
{
    public ValidationError(Error[] errors)
        : base(
            "Validation.General",
            "Một số thông tin chưa hợp lệ, vui lòng kiểm tra lại.")
    {
        Errors = errors;
    }

    public Error[] Errors { get; }

    public static ValidationError FromResults(IEnumerable<Result> results) =>
        new(results.Where(r => r.IsFailure).Select(r => r.Error).ToArray());
}
