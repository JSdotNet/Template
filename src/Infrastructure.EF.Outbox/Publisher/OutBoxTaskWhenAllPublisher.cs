using MediatR;

using Microsoft.EntityFrameworkCore;

using SolutionTemplate.Domain._.Events;
using SolutionTemplate.Infrastructure.EF.Outbox.Entities;

namespace SolutionTemplate.Infrastructure.EF.Outbox.Publisher;

public sealed class OutBoxTaskWhenAllPublisher(DbContext dbContext, ILogger<OutBoxTaskWhenAllPublisher> logger) : INotificationPublisher
{
    public Task Publish(IEnumerable<NotificationHandlerExecutor> handlerExecutors, INotification notification, CancellationToken cancellationToken)
    {
        var tasks = handlerExecutors
            .Select(handler => HandlerHandlerCallback(notification, handler, cancellationToken))
            .ToArray();

        return Task.WhenAll(tasks);
    }

    private async Task HandlerHandlerCallback(INotification notification, NotificationHandlerExecutor handler, CancellationToken cancellationToken)
    {
        if (notification is not IDomainEvent domainEvent)
        {
            // We only handle domain events.
            return;
        }

        var consumer = handler.HandlerInstance.GetType().Name;

        if (await dbContext.Set<OutboxMessageConsumer>().AnyAsync(c =>
                c.OutboxMessageId == domainEvent.Id &&
                c.Name == consumer, cancellationToken))
        {
            // Skip the message, it was already processed. 
            return;
        }

        await handler.HandlerCallback(domainEvent, cancellationToken);

        // Register that the message was processed, so we can skip it next time.
        await RegisterDomainEventHandled(domainEvent, consumer, cancellationToken);
    }

    private async Task RegisterDomainEventHandled(IDomainEvent notification, string consumer, CancellationToken cancellationToken)
    {
        try
        {
            dbContext.Set<OutboxMessageConsumer>().Add(new OutboxMessageConsumer
            {
                OutboxMessageId = notification.Id,
                Name = consumer
            });

            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            // Because the outbox implementation is critical to the system, we want to it works correctly.
            // If registering the consumer fails, we log is with a critical log level.
            logger.FailedToRegisterConsumer(e, notification.Id, consumer);

            // We do not throw the exception. This would trigger a retry on a already correctly processed message.
            // That would be exactly what we try to prevent with this IdempotentDomainEventNotificationHandler
        }
    }
}


public static partial class Log
{
    [LoggerMessage(0, LogLevel.Critical, "Failed to register {Consumer} for {MessageId}")]
    public static partial void FailedToRegisterConsumer(this ILogger logger, Exception exception, Guid messageId, string consumer);
}