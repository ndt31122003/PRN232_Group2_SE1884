using Microsoft.AspNetCore.Http;

namespace PRN232_EbayClone.Infrastructure.FileStorage;

/// <summary>
/// Interface for virus scanning services.
/// Implementations can use ClamAV, VirusTotal, or other antivirus APIs.
/// </summary>
public interface IVirusScanService
{
    /// <summary>
    /// Scans a file stream for viruses.
    /// </summary>
    /// <param name="fileStream">The file stream to scan</param>
    /// <param name="fileName">The file name for logging purposes</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if file is clean, false if virus detected</returns>
    Task<bool> ScanAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if the virus scan service is available/configured.
    /// </summary>
    /// <returns>True if service is available, false otherwise</returns>
    bool IsAvailable();
}
