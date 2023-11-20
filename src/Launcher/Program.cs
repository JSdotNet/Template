using SolutionTemplate.Application;
using SolutionTemplate.Infrastructure.EF;
using SolutionTemplate.Infrastructure.EF.Migrator;
using SolutionTemplate.Infrastructure.EF.Outbox;
using SolutionTemplate.Infrastructure.EF.Outbox.Publisher;
using SolutionTemplate.Presentation.Api;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddOptions();

// Application Insight
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-7.0
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConfiguration(config.GetSection("Logging"));
    
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
    loggingBuilder.AddApplicationInsights();
});
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddSnapshotCollector();

// Health
builder.Services.AddHealthChecks()
    .AddInfrastructureEf(config);
// TODO AspNetCore.HealthChecks.Publisher.ApplicationInsights =>.AddApplicationInsightsPublisher();

builder.Services.AddInfrastructureEfOutbox();
builder.Services.AddApplication<OutBoxTaskWhenAllPublisher>(config);
builder.Services.AddInfrastructureEf(config);
builder.Services.AddPresentation(config);




var app = builder.Build();

// Initialize the database
await app.Services.ApplyMigrations();

app.UseApi(app.Environment, config);

app.Run();




// This partial class is used to provide a hook for the WebApplicationFactory, so that Integration tests can be added.
// I do not like that this is needed, but did not find a better solution.
public partial class Program;