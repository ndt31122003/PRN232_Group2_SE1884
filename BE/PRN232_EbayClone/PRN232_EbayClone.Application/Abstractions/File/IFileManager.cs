using Microsoft.AspNetCore.Http;
using PRN232_EbayClone.Domain.FileMetadata.Entities;

namespace PRN232_EbayClone.Application.Abstractions.File;

public interface IFileManager
{
    Task<Result<string>> UploadFileAsync(IFormFile file);
    Task<Result<IEnumerable<FileMetadata>>> UploadMultipleFilesAsync(IEnumerable<IFormFile> files);
}
