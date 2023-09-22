
using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SolutionTemplate.Application._.Messaging;
using SolutionTemplate.Domain._.Events;
using SolutionTemplate.Infrastructure.EF.Outbox.Entities;

namespace SolutionTemplate.Infrastructure.EF.Outbox.Handler;


public sealed class IdempotentDomainEventNotificationHandler<TDomainEvent> : IDomainEventNotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    private readonly INotificationHandler<TDomainEvent> _decorated;
    private readonly DbContext _dbContext;
    private readonly ILogger<IdempotentDomainEventNotificationHandler<TDomainEvent>> _logger;

    public IdempotentDomainEventNotificationHandler(INotificationHandler<TDomainEvent> decorated, DbContext dbContext, ILogger<IdempotentDomainEventNotificationHandler<TDomainEvent>> logger)
    {
        _decorated = decorated;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        var consumer = _decorated.GetType().Name;

        if (await _dbContext.Set<OutboxMessageConsumer>().AnyAsync(c =>
                c.OutboxMessageId == notification.Id &&
                c.Name == consumer, cancellationToken))
        {
            // Skip the message, it was already processed. 
            return;
        }

        await _decorated.Handle(notification, cancellationToken);

        // Register that the message was processed, so we can skip it next time.
        await RegisterDomainEventHandled(notification, consumer, cancellationToken);
    }

    private async Task RegisterDomainEventHandled(TDomainEvent notification, string consumer, CancellationToken cancellationToken)
    {
        try
        {
            _dbContext.Set<OutboxMessageConsumer>().Add(new OutboxMessageConsumer
            {
                OutboxMessageId = notification.Id,
                Name = consumer
            });

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            // Because the outbox implementation is critical to the system, we want to it works correctly.
            // If registering the consumer fails, we log is with a critical log level.
            _logger.FailedToRegisterConsumer(e, notification.Id, consumer);

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