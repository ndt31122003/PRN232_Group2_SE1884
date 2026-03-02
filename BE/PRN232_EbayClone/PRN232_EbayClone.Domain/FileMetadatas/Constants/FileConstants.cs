namespace PRN232_EbayClone.Domain.FileMetadata.Constants;

public static class FileConstants
{
    public const long MaxFileSizeInBytes = 10 * 1024 * 1024; // 10 MB
    public static readonly string[] AllowedContentTypes =
    [
        "image/jpeg",
        "image/png",
        "image/gif",
        "application/pdf"
    ];
    public static readonly string[] AllowedFileExtensions =
    [
        ".jpg",
        ".jpeg",
        ".png",
        ".gif",
        ".pdf"
    ];
}
