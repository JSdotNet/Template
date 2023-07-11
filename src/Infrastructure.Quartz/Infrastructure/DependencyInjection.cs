using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Quartz;

using SolutionTemplate.Infrastructure.EF.Data;
using SolutionTemplate.Infrastructure.Quartz.Background;

namespace SolutionTemplate.Infrastructure.Quartz;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureQuartz(this IServiceCollection services, IConfiguration _)
    {
        services.AddTransient<DbContext>(provider => provider.GetRequiredService<DataContext>());

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