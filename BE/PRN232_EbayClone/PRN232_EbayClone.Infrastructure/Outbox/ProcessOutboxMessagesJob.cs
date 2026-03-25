using MediatR;
using Microsoft.Extensions.Logging;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Infrastructure.Persistence;
using Newtonsoft.Json;
using Quartz;

namespace PRN232_EbayClone.Infrastructure.Outbox;

[DisallowConcurrentExecution]
public sealed class ProcessOutboxMessagesJob : IJob
{
    private const int MaxRetryCount = 3;
    private const int BatchSize = 20;
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublisher _publisher;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(
        ApplicationDbContext dbContext,
        IPublisher publisher,
        ILogger<ProcessOutboxMessagesJob> logger)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext.OutboxMessages
            .Where(m => m.ProcessedOn == null && m.RetryCount < MaxRetryCount)
            .OrderBy(m => m.OccurredOn)
            .Take(BatchSize)
            .ToListAsync(context.CancellationToken);

        foreach (var outboxMessage in messages)
        {
            try
            {
                var type = Type.GetType(outboxMessage.Type);
                if (type is null)
                {
                    _logger.LogWarning("Unknown event type {EventType} for OutboxMessage {OutboxMessageId}", outboxMessage.Type, outboxMessage.Id);
                    outboxMessage.ProcessedOn = DateTime.UtcNow;
                    outboxMessage.Error = $"Unknown event type: {outboxMessage.Type}";
                    await _dbContext.SaveChangesAsync(context.CancellationToken);
                    continue;
                }

                var domainEvent = (IDomainEvent?)JsonConvert.DeserializeObject(
                    outboxMessage.Content,
                    type,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

                if (domainEvent is null)
                {
                    _logger.LogWarning("Could not deserialize outbox message with ID {OutboxMessageId}", outboxMessage.Id);
                    outboxMessage.ProcessedOn = DateTime.UtcNow;
                    outboxMessage.Error = "Failed to deserialize";
                    await _dbContext.SaveChangesAsync(context.CancellationToken);
                    continue;
                }

                // Mark as processed BEFORE publishing to prevent duplicate sends on retry
                outboxMessage.ProcessedOn = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync(context.CancellationToken);

                await _publisher.Publish(domainEvent, context.CancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing outbox message with ID {OutboxMessageId}", outboxMessage.Id);
                // Roll back ProcessedOn so it can be retried, but only if it wasn't a send-related error
                outboxMessage.ProcessedOn = null;
                outboxMessage.RetryCount++;
                outboxMessage.Error = $"{ex.Message}";
                await _dbContext.SaveChangesAsync(context.CancellationToken);
            }
        }
    }
}