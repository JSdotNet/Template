using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;

namespace SolutionTemplate.Presentation.Api.Health;

public static class HealthCheckExtensions
{
    public static void MapHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/_health/live", new HealthCheckOptions
        {
            // Exclude all checks, just return a 200.
            Predicate = _ => false 

            // Their may be some checks that you want to include here...
        }).AllowAnonymous();

        // The state endpoint returns a 200 if all checks are passing, otherwise a 500.
        // Degraded is considered passing.
        endpoints.MapHealthChecks("/_health/state").AllowAnonymous();

        // The details endpoint returns a 200 if all checks are passing, otherwise a 500.
        // It also returns a json payload with the results of each check.
        endpoints.MapHealthChecks("/_health/details", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).AllowAnonymous();
    }
}