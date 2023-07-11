using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Quartz;

using SolutionTemplate.Infrastructure.EF.Outbox.Background;
using SolutionTemplate.Infrastructure.EF.Outbox.Interceptors;

namespace SolutionTemplate.Infrastructure.EF.Outbox;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureEfOutbox(this IServiceCollection services, IConfiguration _)
    {
        services.AddSingleton<ConvertDomainEventToOutboxInterceptor>();

        services.AddQuartz(configure =>
        {
            var outboxMessageProcessorJobKey = new JobKey(nameof(OutboxMessageProcessor));

            configure.AddJob<OutboxMessageProcessor>(outboxMessageProcessorJobKey);
            configure.AddTrigger(trigger => trigger.ForJob(outboxMessageProcessorJobKey).WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(10))); // TODO Make configurable


            var outboxMessageCleanerJobKey = new JobKey(nameof(OutboxMessageCleaner));

            configure.AddJob<OutboxMessageCleaner>(outboxMessageCleanerJobKey);
            configure.AddTrigger(trigger => trigger.ForJob(outboxMessageCleanerJobKey).WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(10))); // TODO Make configurable

            configure.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService();

        return services;
    }
}