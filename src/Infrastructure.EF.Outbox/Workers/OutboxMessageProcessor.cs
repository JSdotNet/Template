using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Polly;

using Quartz;

using SolutionTemplate.Domain._.Events;
using SolutionTemplate.Infrastructure.EF.Outbox.Entities;

namespace SolutionTemplate.Infrastructure.EF.Outbox.Workers;

[DisallowConcurrentExecution]
internal sealed class OutboxMessageProcessor : IJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    public OutboxMessageProcessor(IServiceProvider serviceProvider, ILogger<OutboxMessageProcessor> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

            var messages = await dbContext.Set<OutboxMessage>()
                .Where(m => m.ProcessedDateUtc == null)
                .OrderBy(m => m.OccurredOnUtc)
                .Take(20)
                .ToListAsync(context.CancellationToken);

            if (!messages.Any())
            {
                return;
            }

            foreach (var message in messages)
            {
                await HandleMessage(message, context.CancellationToken);
            }

            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
        catch (Exception e)
        {
            _logger.OutboxJobError(e);

            // When saving the message processing status fails, the record will be picked up in the next iteration.
            // If some handlers already processed the message, they will be skipped based on the OutboxMessageConsumer registration.
        }
    }

    private async Task HandleMessage(OutboxMessage message, CancellationToken cancellationToken)
    {
        // Make sure each publish runs in its own context. The decorator need to use a different DbContext that this job.
        using var scope = _serviceProvider.CreateScope();
        var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

        var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(message.Content,
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

        if (domainEvent is null)
        {
            _logger.OutboxMessageDeserializeError(message.Id);
            return;
        }

        var retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        var policyResult = await retryPolicy.ExecuteAndCaptureAsync(async () =>
        {
            await publisher.Publish(domainEvent, cancellationToken);
        });

        message.Error = policyResult.FinalException?.Message;
        message.ProcessedDateUtc = DateTime.UtcNow;

        if (policyResult.FinalException is not null)
        {
            _logger.OutboxMessageError(policyResult.FinalException, message.Id);
        }
    }
}



public static partial class Log
{
    [LoggerMessage(0, LogLevel.Critical, "Failed to deserialize Outbox message {MessageId}")]
    public static partial void OutboxMessageDeserializeError(this ILogger logger, Guid messageId);

    [LoggerMessage(1, LogLevel.Critical, "Error processing Outbox message {MessageId}")]
    public static partial void OutboxMessageError(this ILogger logger, Exception exception, Guid messageId);

    [LoggerMessage(2, LogLevel.Critical, "Error processing Outbox messages")]
    public static partial void OutboxJobError(this ILogger logger, Exception exception);
}