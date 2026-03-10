using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Performance.Abstractions;
using PRN232_EbayClone.Application.Performance.Helpers;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.BackgroundJobs;

/// <summary>
/// Background service that checks daily if it's the 20th of the month
/// and re-evaluates all sellers' performance levels.
/// Runs every day at 2:00 AM UTC.
/// </summary>
public sealed class SellerLevelEvaluationJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SellerLevelEvaluationJob> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(24); // Check once per day
    private readonly TimeSpan _evaluationTime = new(2, 0, 0); // 2:00 AM UTC

    public SellerLevelEvaluationJob(
        IServiceProvider serviceProvider,
        ILogger<SellerLevelEvaluationJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Seller Level Evaluation Job started. Will check daily at {Time} UTC.", _evaluationTime);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await WaitForNextScheduledTime(stoppingToken);

                    if (stoppingToken.IsCancellationRequested)
                        break;

                    await PerformEvaluationAsync(stoppingToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex, "Error in Seller Level Evaluation Job");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Graceful shutdown
        }

        _logger.LogInformation("Seller Level Evaluation Job stopped.");
    }

    private async Task WaitForNextScheduledTime(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var nextRun = now.Date.Add(_evaluationTime);

        // If we've passed today's evaluation time, schedule for tomorrow
        if (now >= nextRun)
        {
            nextRun = nextRun.AddDays(1);
        }

        var delay = nextRun - now;
        
        _logger.LogInformation("⏰ Next evaluation check scheduled at {NextRun} UTC (in {Hours}h {Minutes}m)", 
            nextRun, 
            (int)delay.TotalHours, 
            delay.Minutes);

        await Task.Delay(delay, cancellationToken);
    }

    private async Task PerformEvaluationAsync(CancellationToken cancellationToken)
    {
        var nowUtc = DateTime.UtcNow;

        // ✅ Check if today is the 20th
        if (!EvaluationScheduleHelper.IsEvaluationDay(nowUtc))
        {
            _logger.LogInformation("ℹ️ Today ({Date}) is NOT the 20th. Skipping evaluation.", nowUtc.Date);
            return;
        }

        _logger.LogWarning("📊 TODAY IS THE 20TH! Starting monthly seller level evaluation...");

        using var scope = _serviceProvider.CreateScope();
        
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var performanceRepository = scope.ServiceProvider.GetRequiredService<IPerformanceRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        try
        {
            // Get all sellers (you need to implement this method)
            var sellers = await GetAllSellersAsync(userRepository, cancellationToken);
            
            _logger.LogInformation("📋 Found {Count} sellers to evaluate", sellers.Count);

            var updatedCount = 0;
            var errorCount = 0;

            foreach (var seller in sellers)
            {
                try
                {
                    // Get seller's current performance metrics
                    var record = await performanceRepository.GetSellerLevelAsync(
                        seller.Id.Value,
                        nowUtc,
                        cancellationToken);

                    // Update seller's performance level
                    var oldLevel = seller.PerformanceLevel.Name;
                    var newLevel = record.CurrentLevel;

                    if (!string.Equals(oldLevel, newLevel, StringComparison.OrdinalIgnoreCase))
                    {
                        // ✅ FIXED: Use From() instead of FromName()
                        seller.UpdatePerformanceLevel(
                            SellerPerformanceLevel.From(newLevel));
                        
                        userRepository.Update(seller);
                        updatedCount++;

                        _logger.LogWarning(
                            "🔄 Seller {SellerId} level changed: {OldLevel} → {NewLevel}", 
                            seller.Id.Value, 
                            oldLevel, 
                            newLevel);
                    }
                    else
                    {
                        _logger.LogInformation(
                            "✅ Seller {SellerId} level unchanged: {Level}", 
                            seller.Id.Value, 
                            oldLevel);
                    }
                }
                catch (Exception ex)
                {
                    errorCount++;
                    _logger.LogError(ex, "❌ Failed to evaluate seller {SellerId}", seller.Id.Value);
                }
            }

            // Save all changes
            await unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogWarning(
                "✅ Monthly evaluation completed: {Updated} updated, {Errors} errors out of {Total} sellers", 
                updatedCount, 
                errorCount, 
                sellers.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Critical error during monthly evaluation");
            throw;
        }
    }

    private static async Task<System.Collections.Generic.List<Domain.Users.Entities.User>> GetAllSellersAsync(
        IUserRepository userRepository,
        CancellationToken cancellationToken)
    {
        // ⚠️ You need to implement this method in IUserRepository
        // For now, return empty list or implement a workaround
        
        // Option 1: Add GetAllSellersAsync() to IUserRepository
        // Option 2: Query via DbContext directly if available
        
        throw new NotImplementedException(
            "IUserRepository.GetAllSellersAsync() needs to be implemented. " +
            "Add this method to fetch all users with seller role.");
    }
}