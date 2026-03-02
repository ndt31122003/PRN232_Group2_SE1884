using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.FileMetadata.Errors;

public static class FileErrors
{
    public readonly static Error Empty = Error.Failure(
        "File.Empty",
        "File is empty");
    public static Error TooLarge(long size, long maxSize) => Error.Failure(
        "File.TooLarge",
        $"File size {size} bytes exceeds the maximum allowed size of {maxSize} bytes");
    public static Error UnsupportedType(string contentType) => Error.Failure(
        "File.UnsupportedType",
        $"File type {contentType} is not supported");

    public static readonly Error NotFound = Error.Failure(
        "File.NotFound",
        "File not found");
    public static readonly Error EmptyUrl = Error.Failure(
        "File.EmptyUrl",
        "File URL is empty");
}
