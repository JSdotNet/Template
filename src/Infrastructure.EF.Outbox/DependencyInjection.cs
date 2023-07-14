using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Quartz;

using SolutionTemplate.Infrastructure.EF.Outbox.Background;
using SolutionTemplate.Infrastructure.EF.Outbox.Handler;
using SolutionTemplate.Infrastructure.EF.Outbox.Interceptors;
using SolutionTemplate.Infrastructure.EF.Outbox.Options;

namespace SolutionTemplate.Infrastructure.EF.Outbox;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureEfOutbox(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ConvertDomainEventToOutboxInterceptor>();

        var outboxOptions = configuration.GetSection(nameof(OutboxOptions)).Get<OutboxOptions>();

        services.AddQuartz(configure =>
        {
            var outboxMessageProcessorJobKey = new JobKey(nameof(OutboxMessageProcessor));

            configure.AddJob<OutboxMessageProcessor>(outboxMessageProcessorJobKey);
            configure.AddTrigger(trigger => trigger.ForJob(outboxMessageProcessorJobKey)
                .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(outboxOptions?.MessageProcessorIntervalInSeconds ?? 10).RepeatForever()));


            var outboxMessageCleanerJobKey = new JobKey(nameof(OutboxMessageCleaner));

            configure.AddJob<OutboxMessageCleaner>(outboxMessageCleanerJobKey);
            configure.AddTrigger(trigger => trigger.ForJob(outboxMessageCleanerJobKey)
                .WithSimpleSchedule(schedule => schedule.WithIntervalInHours(outboxOptions?.MessageCleanupIntervalDays ?? 28).RepeatForever()));

            configure.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService();
        
        // TODO Dependency MediatR
        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventNotificationHandler<>));

        return services;
    }
}