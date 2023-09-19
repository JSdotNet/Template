using System.Globalization;

using Serilog;

using SolutionTemplate.Application;
using SolutionTemplate.Infrastructure.EF;
using SolutionTemplate.Infrastructure.EF.Migrator;
using SolutionTemplate.Presentation.Api;



var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// TODO Review... 
builder.Services.AddOptions();

// Logging
// TODO Try with Pure .net https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-7.0
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console(formatProvider: CultureInfo.CurrentCulture);
});

// Health
builder.Services.AddHealthChecks()
    .AddInfrastructureEf(config)
    .AddApplication(config);

// TODO AspNetCore.HealthChecks.Publisher.ApplicationInsights =>.AddApplicationInsightsPublisher();


builder.Services.AddApplication(config);
builder.Services.AddInfrastructureEf(config);
builder.Services.AddPresentation(config);


var app = builder.Build();

// Initialize the database
await app.Services.ApplyMigrations();

app.UseApi(app.Environment, config);

app.Run();




// This partial class is used to provide a hook for the WebApplicationFactory, so that Integration tests can be added.
// I do not like that this is needed, but did not find a better solution.
public partial class Program { }