using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Quartz;

using SolutionTemplate.Domain._;
using SolutionTemplate.Infrastructure.EF.Outbox.Data;

namespace SolutionTemplate.Infrastructure.EF.Outbox.Background;

[DisallowConcurrentExecution]
internal sealed class OutboxMessageProcessor : IJob
{
    private readonly DbContext _dbContext;
    private readonly IPublisher _publisher;
    private readonly ILogger _logger;

    public OutboxMessageProcessor(DbContext dbContext, IPublisher publisher, ILogger logger)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var messages = await _dbContext.Set<OutboxMessage>()
                .Where(m => m.ProcessedDateUtc == null)
                .OrderBy(m => m.OccurredOnUtc)
                .Take(20)
                .ToListAsync(context.CancellationToken);

            foreach (var message in messages)
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(message.Content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All});
                
                if (domainEvent is null)
                {
                    _logger.OutboxMessageDeserializeError(message.Id);
                    continue;
                }

                await HandleEvent(context, domainEvent, message);

                message.ProcessedDateUtc = DateTime.UtcNow;
            }

            await _dbContext.SaveChangesAsync(context.CancellationToken);
        }
        catch (Exception e)
        {
            _logger.OutboxJobError(e);
        }
    }

    private async Task HandleEvent(IJobExecutionContext context, IDomainEvent domainEvent, OutboxMessage message)
    {
        try
        {
            await _publisher.Publish(domainEvent, context.CancellationToken);
            message.ProcessedDateUtc = DateTime.UtcNow;
        }
        catch (Exception e)
        {
            // I catch any exception here to prevent other events from not being processed.
            // If an event has multiple handlers, it is still possible that one of them will fail and the others will succeed.
            // In that case, the event will be processed again later...

            _logger.OutboxMessageError(e, message.Id);

            message.Error = e.Message;
        }
    }
}



public static partial class Log
{
    [LoggerMessage(0, LogLevel.Critical, "Failed to deserialize Outbox message {MessageId}")]
    public static partial void OutboxMessageDeserializeError(this ILogger logger, Guid messageId);


    [LoggerMessage(1, LogLevel.Critical, "Error processing Outbox messages")]
    public static partial void OutboxJobError(this ILogger logger, Exception exception);


    [LoggerMessage(2, LogLevel.Critical, "Error handling Outbox message {messageId}")]
    public static partial void OutboxMessageError(this ILogger logger, Exception exception, Guid messageId);
}