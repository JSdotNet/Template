using Microsoft.EntityFrameworkCore;

using SolutionTemplate.Domain;
using SolutionTemplate.Domain.Models;

namespace SolutionTemplate.Infrastructure.EF.Data;

internal sealed class DataContext : DbContext, IUnitOfWork
{
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<Author> Authors => Set<Author>();

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}