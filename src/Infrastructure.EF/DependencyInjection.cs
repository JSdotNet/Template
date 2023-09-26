using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SolutionTemplate.Domain;
using SolutionTemplate.Domain.Repository;

using SolutionTemplate.Infrastructure.EF.Data;
using SolutionTemplate.Infrastructure.EF.Migrator;
using SolutionTemplate.Infrastructure.EF.Repository;

namespace SolutionTemplate.Infrastructure.EF;

public static class DependencyInjection
{
    public static void AddInfrastructureEf(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddDbContext<DataContext>((provider, options) =>
        {
            var connectionString = configuration.GetConnectionString("Database")!;
            options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(assembly.GetName().Name);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            });

            // Get all registered interceptors            
            var interceptors = provider.GetServices<SaveChangesInterceptor>().ToList();
            options.AddInterceptors(interceptors);
        });

        // The outbox pattern requires the DbContext
        services.AddTransient<DbContext>(provider => provider.GetRequiredService<DataContext>());

        services.AddScoped<IReadOnlyDataContext, ReadOnlyDataContext>();

        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();

        services.AddScoped<IUnitOfWork>(provider => provider.GetService<DataContext>()!);

        services.AddTransient<IDatabaseMigrator, DatabaseMigrator>();
    }

    public static void AddInfrastructureEf(this IHealthChecksBuilder builder, IConfiguration configuration) // TODO Options
    {
        builder.AddSqlServer(configuration.GetConnectionString("Database")!);
        builder.AddDbContextCheck<DataContext>();
    }
}