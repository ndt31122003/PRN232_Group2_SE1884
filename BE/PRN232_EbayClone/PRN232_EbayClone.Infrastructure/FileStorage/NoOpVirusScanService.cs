namespace PRN232_EbayClone.Infrastructure.FileStorage;

/// <summary>
/// No-operation virus scan service used when virus scanning is not configured.
/// Always returns true (file is clean) without performing any actual scanning.
/// </summary>
public sealed class NoOpVirusScanService : IVirusScanService
{
    public Task<bool> ScanAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        // No-op: always return true (file is clean)
        return Task.FromResult(true);
    }

    public bool IsAvailable()
    {
        return false;
    }
}
