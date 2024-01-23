using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using SolutionTemplate.Infrastructure.EF.Outbox.Interceptors;
using SolutionTemplate.Infrastructure.EF.Outbox.Metrics;
using SolutionTemplate.Infrastructure.EF.Outbox.Options;
using SolutionTemplate.Infrastructure.EF.Outbox.Workers;

namespace SolutionTemplate.Infrastructure.EF.Outbox;

public static class DependencyInjection
{
    public static void AddInfrastructureEfOutbox(this IServiceCollection services)
    {
        services.AddOptions<OutboxOptions>();
        services.AddTransient<IConfigureOptions<OutboxOptions>, OutboxOptionsSetup>();

        services.AddSingleton<SaveChangesInterceptor, ConvertDomainEventToOutboxInterceptor>();

        services.AddHostedService<OutboxMessageProcessor>();
        services.AddHostedService<OutboxMessageCleaner>();

        services.AddSingleton<OutboxMetrics>();
    }
}