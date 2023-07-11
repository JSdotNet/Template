using SolutionTemplate.Application;
using SolutionTemplate.Infrastructure.EF;
using SolutionTemplate.Presentation.Api;
using SolutionTemplate.WebApi.Middleware;

namespace SolutionTemplate.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ExceptionHandlerMiddleware>();

        services.AddInfrastructureEf(configuration)
                .AddApplication(configuration)
                .AddPresentation(configuration);

        return services;
    }


    public static IHealthChecksBuilder AddHealthChecks(this IHealthChecksBuilder builder, IConfiguration configuration)
    {
        builder.AddInfrastructureEf(configuration)
               .AddApplication(configuration)
               .AddPresentation(configuration);

        // TODO  AspNetCore.HealthChecks.Publisher.ApplicationInsights =>.AddApplicationInsightsPublisher();

        return builder;
    }
}