using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PRN232_EbayClone.Infrastructure.Services.ShippingProvider;

public sealed class ChromiumPreloadHostedService : IHostedService
{
    private readonly ILogger<ChromiumPreloadHostedService> _logger;

    public ChromiumPreloadHostedService(ILogger<ChromiumPreloadHostedService> logger)
    {
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();
        try
        {
            _logger.LogInformation("Ensuring Chromium executable is available for PDF rendering...");
            var executablePath = await PdfShippingLabelRenderer.EnsureChromiumExecutableAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Chromium executable ready at {ExecutablePath}", executablePath);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Chromium prefetch was cancelled during startup.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to prefetch Chromium executable for PDF rendering.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
