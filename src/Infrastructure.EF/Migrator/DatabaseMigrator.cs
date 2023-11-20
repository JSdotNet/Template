using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SolutionTemplate.Infrastructure.EF.Data;

namespace SolutionTemplate.Infrastructure.EF.Migrator;

internal sealed class DatabaseMigrator(IServiceProvider serviceProvider) : IDatabaseMigrator
{
    public async Task Execute(CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();


        var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        //// turn off timeout for initial seeding
        //context.Database.SetCommandTimeout(TimeSpan.FromMinutes(30));


        await context.Database.MigrateAsync(cancellationToken);
    }
}
