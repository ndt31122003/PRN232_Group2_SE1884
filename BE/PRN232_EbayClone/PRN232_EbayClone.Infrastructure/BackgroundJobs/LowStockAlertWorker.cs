using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Listings.Inventory.Services;

namespace PRN232_EbayClone.Infrastructure.BackgroundJobs;

public sealed class LowStockAlertWorker : BackgroundService
{
    private static readonly TimeSpan ScanInterval = TimeSpan.FromMinutes(5);

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<LowStockAlertWorker> _logger;

    public LowStockAlertWorker(IServiceProvider serviceProvider, ILogger<LowStockAlertWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Low-stock alert worker started. Scan interval: {IntervalMinutes} minutes.", ScanInterval.TotalMinutes);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ScanAndSendAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Low-stock alert worker encountered an unexpected error.");
            }

            await Task.Delay(ScanInterval, stoppingToken);
        }

        _logger.LogInformation("Low-stock alert worker stopped.");
    }

    private async Task ScanAndSendAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var inventoryRepository = scope.ServiceProvider.GetRequiredService<IInventoryRepository>();
        var listingRepository = scope.ServiceProvider.GetRequiredService<IListingRepository>();
        var notifier = scope.ServiceProvider.GetRequiredService<IInventoryLowStockNotifier>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var candidates = await inventoryRepository.GetPendingLowStockAlertsAsync(cancellationToken);
        if (candidates.Count == 0)
        {
            _logger.LogDebug("Low-stock alert worker found no pending alerts.");
            return;
        }

        var sentCount = 0;

        foreach (var inventory in candidates)
        {
            try
            {
                var listing = await listingRepository.GetByIdAsync(inventory.ListingId.Value, cancellationToken);
                if (listing is null)
                {
                    _logger.LogWarning("Low-stock alert skipped because listing {ListingId} was not found.", inventory.ListingId.Value);
                    continue;
                }

                if (await notifier.NotifyIfNeededAsync(inventory, listing.Title, listing.Sku, cancellationToken))
                {
                    inventoryRepository.Update(inventory);
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                    sentCount++;
                }
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                _logger.LogError(exception, "Failed to send low-stock alert for listing {ListingId}.", inventory.ListingId.Value);
            }
        }

        _logger.LogInformation("Low-stock alert worker completed scan. {SentCount} alerts sent from {CandidateCount} candidates.", sentCount, candidates.Count);
    }
}