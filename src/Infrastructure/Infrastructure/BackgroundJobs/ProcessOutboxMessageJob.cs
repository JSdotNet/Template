using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Quartz;

using SolutionTemplate.Domain._;
using SolutionTemplate.Infrastructure.EF.Data;
using SolutionTemplate.Infrastructure.EF.Outbox;

namespace SolutionTemplate.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessageJob : IJob
{
    private readonly DataContext _dbContext;
    private readonly IPublisher _publisher;
    private readonly ILogger _logger;

    public ProcessOutboxMessageJob(DataContext dbContext, IPublisher publisher, ILogger logger)
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

                await _publisher.Publish(domainEvent, context.CancellationToken);

                message.ProcessedDateUtc = DateTime.UtcNow;
            }

            await _dbContext.SaveChangesAsync(context.CancellationToken);
        }
        catch (Exception e)
        {
            _logger.OutboxJobError(e);
        }
    }
}



public static partial class Log
{
    [LoggerMessage(2, LogLevel.Critical, "Failed to deserialize outbox message {MessageId}")]
    public static partial void OutboxMessageDeserializeError(this ILogger logger, Guid messageId);


    [LoggerMessage(3, LogLevel.Critical, "Error processing Outbox messages")]
    public static partial void OutboxJobError(this ILogger logger, Exception exception);
}