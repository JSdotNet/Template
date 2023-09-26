using System.Data.Common;

using MediatR;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Respawn;
using Respawn.Graph;

using SolutionTemplate.Application._.Messaging;
using SolutionTemplate.Domain;
using SolutionTemplate.Infrastructure.EF.Data;
using SolutionTemplate.Infrastructure.EF.Migrator;
using SolutionTemplate.Infrastructure.EF.Outbox.Workers;

using Testcontainers.MsSql;

namespace SolutionTemplate.Integration.Tests.Helpers;

public sealed class IntegrationTestApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    private string ConnectionString => _dbContainer.GetConnectionString();


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        Environment.SetEnvironmentVariable("ConnectionStrings__Database", ConnectionString);
        Environment.SetEnvironmentVariable("OutboxOptions__MessageProcessorIntervalInSeconds", "1");
        Environment.SetEnvironmentVariable("OutboxOptions__MessageProcessorSimultaneousMessages", "10");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IDomainEventNotificationHandler<>));
            services.RemoveAll(typeof(INotificationHandler<DomainEvents.AuthorCreated>));
            services.RemoveAll(typeof(INotificationHandler<DomainEvents.ArticleCreated>));

            services.RemoveAll(typeof(DbContextOptions<DbContext>));
            services.RemoveAll(typeof(IDatabaseMigrator));

            services.RemoveAll(typeof(OutboxMessageCleaner));

            services.AddTransient<INotificationHandler<DomainEvents.AuthorCreated>, OutboxEventHandler<DomainEvents.AuthorCreated>>();
            services.AddTransient<INotificationHandler<DomainEvents.ArticleCreated>, OutboxEventHandler<DomainEvents.ArticleCreated>>();
            services.AddSingleton<IDomainEventLogger, DomainEventLogger>();
        });
    }

    public Task ResetStateAsync() => _respawner.ResetAsync(_dbConnection);


    public async Task InitializeAsync()
    {
        // Start the database container
        await _dbContainer.StartAsync();

        // Ensure database is created and seeded
        await using var context = new DataContext(new DbContextOptionsBuilder<DataContext>().UseSqlServer(ConnectionString).Options);
        await context.Database.EnsureCreatedAsync();

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
