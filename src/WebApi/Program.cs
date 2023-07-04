using System.Globalization;

using Serilog;

using SolutionTemplate.Infrastructure.EF.Migrator;
using SolutionTemplate.Presentation.Api;
using SolutionTemplate.WebApi;
using SolutionTemplate.WebApi.Health;
using SolutionTemplate.WebApi.Middleware;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Logging
// TODO Try with Pure .net https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-7.0
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console(formatProvider: CultureInfo.CurrentCulture);
});

// Setup controllers in the presentation project
builder.Services
    .AddControllers()
    .AddApplicationPart(SolutionTemplate.Presentation.Api.AssemblyReference.Assembly);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
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
        Title = "WebApi",
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

// Health
builder.Services.AddHealthChecks().AddHealthChecks(config);

builder.Services.AddDependencies(config);


var app = builder.Build();

// Initialize the database
await app.Services.ApplyMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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

app.UseMiddleware<ExceptionHandlerMiddleware>();

//app.UseHttpsRedirection();
//app.UseAuthorization();

app.MapControllers();
app.MapEndpoints(config);
app.MapHealthChecks();

app.Run();




// This partial class is used to provide a hook for the WebApplicationFactory, so that Integration tests can be added.
// I do not like that this is needed, but did not find a better solution.
public partial class Program { }