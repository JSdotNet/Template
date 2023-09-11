using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SolutionTemplate.Presentation.Api.Modules;

namespace SolutionTemplate.Presentation.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration _)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        

        return services;
    }


    public static IHealthChecksBuilder AddPresentation(this IHealthChecksBuilder builder, IConfiguration _)
    {
        return builder;
    }

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints, IConfiguration options)
    {
        return endpoints.MapArticleEndpoints(options);
    }
}