using System.Data.Common;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Respawn;
using Respawn.Graph;

using SolutionTemplate.Infrastructure.EF.Migrator;

using Testcontainers.MsSql;

using Xunit;

namespace SolutionTemplate.Integration.Tests.Helpers;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    public HttpClient HttpClient { get; private set; } = default!;
    public string ConnectionString => _dbContainer.GetConnectionString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Instead of using environment variables to bootstrap our application configuration, we can implement a custom WebApplicationFactory<TEntryPoint>
        // that overrides the ConfigureWebHost(IWebHostBuilder) method to add a WeatherDataContext to the service collection.
        Environment.SetEnvironmentVariable("ASPNETCORE_URLS", "https://+");
        Environment.SetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Path", "certificate.crt");
        Environment.SetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Password", "password");
        Environment.SetEnvironmentVariable("ConnectionStrings__Database", _dbContainer.GetConnectionString());

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<DbContext>));
            services.RemoveAll(typeof(IDatabaseMigrator));
        });
    }


    public Task ResetStateAsync() => _respawner.ResetAsync(_dbConnection);


    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await TestData.SeedTestData(ConnectionString);

        InitializeHttpClient();

        await InitializeRespawner();
    }

    private void InitializeHttpClient()
    {
        _dbConnection = new SqlConnection(_dbContainer.GetConnectionString());
        HttpClient = CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    private async Task InitializeRespawner()
    {
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
    }
}
