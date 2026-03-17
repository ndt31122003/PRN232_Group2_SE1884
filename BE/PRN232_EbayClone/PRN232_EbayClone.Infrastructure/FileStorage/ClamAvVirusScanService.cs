using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PRN232_EbayClone.Infrastructure.FileStorage;

/// <summary>
/// Virus scan service implementation using ClamAV.
/// Connects to a ClamAV daemon to scan files for viruses.
/// </summary>
public sealed class ClamAvVirusScanService : IVirusScanService
{
    private readonly ClamAvConfiguration _configuration;
    private readonly ILogger<ClamAvVirusScanService> _logger;

    public ClamAvVirusScanService(IOptions<ClamAvConfiguration> options, ILogger<ClamAvVirusScanService> logger)
    {
        _configuration = options.Value;
        _logger = logger;
    }

    public async Task<bool> ScanAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        if (!IsAvailable())
        {
            _logger.LogWarning("ClamAV is not configured. Skipping virus scan for file: {FileName}", fileName);
            return true;
        }

        try
        {
            using var client = new System.Net.Sockets.TcpClient();
            await client.ConnectAsync(_configuration.Host, _configuration.Port, cancellationToken);

            using var stream = client.GetStream();

            // Send INSTREAM command to ClamAV
            var instreamCommand = "nINSTREAM\n";
            var commandBytes = System.Text.Encoding.ASCII.GetBytes(instreamCommand);
            await stream.WriteAsync(commandBytes, 0, commandBytes.Length, cancellationToken);

            // Send file data in chunks
            var buffer = new byte[_configuration.ChunkSize];
            int bytesRead;

            while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
            {
                // Send chunk size (4 bytes, big-endian)
                var sizeBytes = BitConverter.GetBytes(bytesRead);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(sizeBytes);

                await stream.WriteAsync(sizeBytes, 0, 4, cancellationToken);
                await stream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
            }

            // Send end-of-stream marker (0 bytes)
            var endMarker = new byte[] { 0, 0, 0, 0 };
            await stream.WriteAsync(endMarker, 0, 4, cancellationToken);

            // Read response
            var responseBuffer = new byte[1024];
            var responseLength = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length, cancellationToken);
            var response = System.Text.Encoding.ASCII.GetString(responseBuffer, 0, responseLength).Trim();

            _logger.LogInformation("ClamAV scan result for {FileName}: {Response}", fileName, response);

            // Check if file is clean
            // ClamAV returns "stream: OK" if clean, "stream: [virus name] FOUND" if infected
            var isClean = response.Contains("OK") && !response.Contains("FOUND");

            if (!isClean)
            {
                _logger.LogWarning("Virus detected in file {FileName}: {Response}", fileName, response);
            }

            return isClean;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scanning file {FileName} with ClamAV", fileName);
            // Fail open: if scanning fails, allow the file (log the error for investigation)
            return true;
        }
    }

    public bool IsAvailable()
    {
        return !string.IsNullOrWhiteSpace(_configuration.Host) && _configuration.Port > 0;
    }
}

/// <summary>
/// Configuration for ClamAV virus scanning service.
/// </summary>
public sealed class ClamAvConfiguration
{
    /// <summary>
    /// ClamAV daemon host address (e.g., "localhost" or "192.168.1.100")
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// ClamAV daemon port (default: 3310)
    /// </summary>
    public int Port { get; set; } = 3310;

    /// <summary>
    /// Chunk size for streaming file data to ClamAV (default: 4096 bytes)
    /// </summary>
    public int ChunkSize { get; set; } = 4096;

    /// <summary>
    /// Timeout for ClamAV connection in milliseconds (default: 30000ms)
    /// </summary>
    public int TimeoutMs { get; set; } = 30000;
}
