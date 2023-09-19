using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Quartz;

using SolutionTemplate.Infrastructure.EF.Outbox.Handler;
using SolutionTemplate.Infrastructure.EF.Outbox.Interceptors;
using SolutionTemplate.Infrastructure.EF.Outbox.Options;
using SolutionTemplate.Infrastructure.EF.Outbox.Workers;

namespace SolutionTemplate.Infrastructure.EF.Outbox;

public static class DependencyInjection
{
    public static void AddInfrastructureEfOutbox(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<OutboxOptions>();
        services.AddTransient<IConfigureOptions<OutboxOptions>, OutboxOptionsSetup>();
        
        services.AddSingleton<ConvertDomainEventToOutboxInterceptor>();

        // TODO Replace with custom background service
        var outboxOptions = configuration.GetSection(nameof(OutboxOptions)).Get<OutboxOptions>();

        services.AddQuartz(configure =>
        {
            // TODO Can I move this to a separate initialization class? Can I use IOptionsMonitor<T>?
            var outboxMessageProcessorJobKey = new JobKey(nameof(OutboxMessageProcessor));

            configure.AddJob<OutboxMessageProcessor>(outboxMessageProcessorJobKey);
            configure.AddTrigger(trigger => trigger.ForJob(outboxMessageProcessorJobKey)
                .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(outboxOptions?.MessageProcessorIntervalInSeconds ?? 10).RepeatForever()));


            var outboxMessageCleanerJobKey = new JobKey(nameof(OutboxMessageCleaner));

            // TODO I should probably use a Cron schedule here
            configure.AddJob<OutboxMessageCleaner>(outboxMessageCleanerJobKey);
            configure.AddTrigger(trigger => trigger.ForJob(outboxMessageCleanerJobKey)
                .WithSimpleSchedule(schedule => schedule.WithIntervalInHours(outboxOptions?.MessageCleanupIntervalDays ?? 28).RepeatForever()));
        });

        services.AddQuartzHostedService();

        // TODO Try to replace this with custom implementation of INotificationPublisher
        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventNotificationHandler<>));
    }
}