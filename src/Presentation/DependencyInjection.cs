using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SolutionTemplate.Presentation.Api._;
using SolutionTemplate.Presentation.Api.Health;
using SolutionTemplate.Presentation.Api.Middleware;
using SolutionTemplate.Presentation.Api.Modules;

namespace SolutionTemplate.Presentation.Api;

public static class DependencyInjection
{
    public static void AddPresentation(this IServiceCollection services, IConfiguration _)
    {
        services.AddExceptionHandler<ExceptionHandler>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            // Use full name for schema Id's, this prevents conflicts with usage of command and query classes with the same name
            options.CustomSchemaIds(x =>
            {
                var name = x.FullName?.Replace("+", ".");

                // TODO Review ...
                var lastPart = x.Namespace!.Split('.').Length == 1
                    ? x.Namespace
                    : x.Namespace.Split('.').Last();

                var otherParts = x.Namespace.Replace(lastPart, string.Empty);
                if (otherParts.Length > 0)
                    name = name?.Replace(otherParts, string.Empty);


                return name;
            });

            // TODO Review ...
            //var presentationDocumentationFile = $"{SolutionTemplate.Presentation.Api.AssemblyReference.Assembly.GetName().Name}.xml";
            //var presentationDocumentationPath = Path.Combine(AppContext.BaseDirectory, presentationDocumentationFile);
            //c.IncludeXmlComments(presentationDocumentationPath);


            options.SwaggerDoc("v1", new()
            {
                Version = "v1",
                Title = "Template API",
                Description = "TODO ...",

            });

            // Filters
            options.OperationFilter<ExceptionOperationFilter>();
            // TODO options.DocumentFilter<PresentationFilter>();

            // Add health check endpoint to swagger
            // TODO options.DocumentFilter<HealthChecksFilter>();

            // Use this for problem solving...
            //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        });
    }

    public static void UseApi(this WebApplication app, IWebHostEnvironment environment, IConfiguration configuration)
    {
        // Configure the HTTP request pipeline.
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // TODO Review ...
                //options.DefaultModelExpandDepth(0);
                //options.DefaultModelsExpandDepth(0);
            });
            app.UseDeveloperExceptionPage();
        }

        //app.UseHttpsRedirection();
        //app.UseAuthorization();

        app.MapEndpoints(configuration);
        app.MapHealthChecks();
    }


    public static void MapEndpoints(this IEndpointRouteBuilder endpoints, IConfiguration options)
    {
        endpoints.MapArticleEndpoints(options);
        endpoints.MapAuthorEndpoints(options);
    }
}