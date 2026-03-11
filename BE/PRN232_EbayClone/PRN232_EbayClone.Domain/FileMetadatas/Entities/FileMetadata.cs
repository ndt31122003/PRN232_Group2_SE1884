using PRN232_EbayClone.Domain.FileMetadata.Constants;
using PRN232_EbayClone.Domain.FileMetadata.Errors;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.FileMetadata.Entities;

public sealed class FileMetadata(Guid id) : AggregateRoot<Guid>(id)
{
    public Guid? LinkedEntityId { get; private set; } = null;
    public string Url { get; private set; } = null!;
    public string FileName { get; private set; } = null!;
    public string ContentType { get; private set; } = null!;
    public long Size { get; private set; }

    public static Result<FileMetadata> Create(
        string url,
        string fileName,
        string contentType,
        long size,
        Guid? linkedEntityId = default)
    {
        if (string.IsNullOrWhiteSpace(url))
            return FileErrors.EmptyUrl;

        if (size > FileConstants.MaxFileSizeInBytes)
            return FileErrors.TooLarge(size, FileConstants.MaxFileSizeInBytes);

        if (!FileConstants.AllowedContentTypes.Contains(contentType))
            return FileErrors.UnsupportedType(contentType);

        var fileMetadata = new FileMetadata(Guid.NewGuid())
        {
            LinkedEntityId = linkedEntityId,
            Url = url,
            FileName = fileName,
            ContentType = contentType,
            Size = size
        };
        return fileMetadata;
    }

    public void UpdateLinkedEntityId(Guid id)
        => LinkedEntityId = id;
}
