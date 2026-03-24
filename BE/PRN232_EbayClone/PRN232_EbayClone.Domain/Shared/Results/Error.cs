namespace PRN232_EbayClone.Domain.Shared.Results;

public record Error
{
    public static readonly Error None = new(
        string.Empty,
        string.Empty);
    public static readonly Error NullValue = new(
        "General.Null",
        "Null value was provided");

    public Error(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public string Code { get; }

    public string Description { get; }

    public static Error Failure(string code, string description) =>
        new(code, description);

    public static Error Validation(string code, string description) =>
        new(code, description);

    public static Error NotFound(string code, string description) =>
        new(code, description);

    public static Error Unauthorized(string description) =>
        new("General.Unauthorized", description);
}
