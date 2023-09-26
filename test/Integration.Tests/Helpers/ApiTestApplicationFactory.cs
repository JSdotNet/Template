using System.Data.Common;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Respawn;
using Respawn.Graph;

using SolutionTemplate.Infrastructure.EF.Data;
using SolutionTemplate.Infrastructure.EF.Migrator;

using Testcontainers.MsSql;

namespace SolutionTemplate.Integration.Tests.Helpers;

public sealed class ApiTestApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    public HttpClient HttpClient { get; private set; } = default!;
    private string ConnectionString => _dbContainer.GetConnectionString();
 
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Instead of using environment variables to bootstrap our application configuration, we can implement a custom WebApplicationFactory<TEntryPoint>
        // that overrides the ConfigureWebHost(IWebHostBuilder) method to add a WeatherDataContext to the service collection.
        Environment.SetEnvironmentVariable("ASPNETCORE_URLS", "https://+");
        Environment.SetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Path", "certificate.crt");
        Environment.SetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Password", "password");

        Environment.SetEnvironmentVariable("ConnectionStrings__Database", _dbContainer.GetConnectionString());
        Environment.SetEnvironmentVariable("OutboxOptions__MessageProcessorIntervalInSeconds", "1");
        Environment.SetEnvironmentVariable("OutboxOptions__MessageProcessorSimultaneousMessages", "10");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<DbContext>));
            services.RemoveAll(typeof(IDatabaseMigrator));
        });

    }

    public async Task ResetStateAsync() 
    {
        await _respawner.ResetAsync(_dbConnection);

        await using var context = new DataContext(new DbContextOptionsBuilder<DataContext>().UseSqlServer(ConnectionString).Options);
        await TestData.SeedTestData(context);
    }


    public async Task InitializeAsync()
    {
        // Start the database container
        await _dbContainer.StartAsync();

        // Ensure database is created and seeded
        await using var context = new DataContext(new DbContextOptionsBuilder<DataContext>().UseSqlServer(ConnectionString).Options);
        await context.Database.EnsureCreatedAsync();

        // Seed test data
        await TestData.SeedTestData(context);

        // Create HttpClient
        HttpClient = CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Setup respawner
        _dbConnection = new SqlConnection(ConnectionString);
        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            TablesToIgnore = new Table[] { "sysdiagrams", "__EFMigrationsHistory", },
        });
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();

        await _dbConnection.DisposeAsync();
    }
}
