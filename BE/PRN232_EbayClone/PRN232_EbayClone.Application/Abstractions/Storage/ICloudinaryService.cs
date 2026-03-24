using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Abstractions.Storage;

public interface ICloudinaryService
{
    Task<Result<string>> UploadImageAsync(string base64Image, CancellationToken cancellationToken = default);
}
