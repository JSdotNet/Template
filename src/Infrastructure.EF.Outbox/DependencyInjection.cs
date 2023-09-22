using MediatR;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using SolutionTemplate.Infrastructure.EF.Outbox.Handler;
using SolutionTemplate.Infrastructure.EF.Outbox.Interceptors;
using SolutionTemplate.Infrastructure.EF.Outbox.Options;
using SolutionTemplate.Infrastructure.EF.Outbox.Workers;

namespace SolutionTemplate.Infrastructure.EF.Outbox;

public static class DependencyInjection
{
    public static void AddInfrastructureEfOutbox(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<OutboxOptions>(); //.Configure(options => configuration.GetSection(nameof(OutboxOptions)).Bind(options));
        services.AddTransient<IConfigureOptions<OutboxOptions>, OutboxOptionsSetup>();

        services.AddSingleton<SaveChangesInterceptor, ConvertDomainEventToOutboxInterceptor>();

        services.AddHostedService<OutboxMessageProcessor>();
        services.AddHostedService<OutboxMessageCleaner>();

        // TODO Try to replace this with custom implementation of INotificationPublisher
        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventNotificationHandler<>));
    }
}