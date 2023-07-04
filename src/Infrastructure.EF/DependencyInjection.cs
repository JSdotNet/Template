using Microsoft.EntityFrameworkCore;
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
    public static IServiceCollection AddInfrastructureEf(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddDbContext<DataContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Database");
            options.UseSqlServer(connectionString,
               sqlServerOptionsAction: sqlOptions =>
               {
                    sqlOptions.MigrationsAssembly(assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
               });
        });

        services.AddScoped<IReadOnlyDataContext, ReadOnlyDataContext>();

        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();

        services.AddScoped<IUnitOfWork>(provider => provider.GetService<DataContext>()!);

        services.AddTransient<IDatabaseMigrator, DatabaseMigrator>();

        return services;
    }

    public static IHealthChecksBuilder AddInfrastructureEf(this IHealthChecksBuilder builder, IConfiguration configuration)
    {
        builder.AddSqlServer(configuration.GetConnectionString("Database")!);
        builder.AddDbContextCheck<DataContext>();

        return builder;
    }
}