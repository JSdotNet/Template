using FluentValidation;

using MediatR;
using MediatR.NotificationPublishers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SolutionTemplate.Application._.Behaviors;

namespace SolutionTemplate.Application;


public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration _)
    {
        // TODO Use Scrutor ???
        //services.Scan(scan => scan
        //    .FromAssemblies(AssemblyReference.Assembly)
        //    .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
        //    .AsImplementedInterfaces()
        //    .WithTransientLifetime());

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(AssemblyReference.Assembly);

            configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ApplicationErrorPipelineBehavior<,>));
            configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkPipelineBehavior<,>));

            configuration.NotificationPublisher = new TaskWhenAllPublisher();
        });

        services.AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);

        return services;
    }

    public static IHealthChecksBuilder AddApplication(this IHealthChecksBuilder builder, IConfiguration _)
    {
        return builder;
    }
}
