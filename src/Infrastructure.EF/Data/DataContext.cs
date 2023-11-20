using Microsoft.EntityFrameworkCore;

using SolutionTemplate.Domain;
using SolutionTemplate.Domain.Models;

namespace SolutionTemplate.Infrastructure.EF.Data;

public sealed class DataContext(DbContextOptions<DataContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<Author> Authors => Set<Author>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.LogTo(Console.WriteLine, minimumLevel: LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(Outbox.AssemblyReference.Assembly);
    }
}