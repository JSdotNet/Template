using FluentValidation;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SolutionTemplate.Application._.Behaviors;

namespace SolutionTemplate.Application;


public static class DependencyInjection
{
    public static void AddApplication<TPublisher>(this IServiceCollection services, IConfiguration _)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(AssemblyReference.Assembly);

            configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ApplicationErrorPipelineBehavior<,>));
            configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkPipelineBehavior<,>));

            configuration.NotificationPublisherType = typeof(TPublisher);
        });

        services.AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);
    }
}