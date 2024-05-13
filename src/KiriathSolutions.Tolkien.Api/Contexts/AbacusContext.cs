using Microsoft.EntityFrameworkCore;
using KiriathSolutions.Tolkien.Api.Entities;

namespace KiriathSolutions.Tolkien.Api.Contexts;

public class AbacusContext : DbContext, IAbacusContext
{
    public DbSet<AccountHistory> AccountHistories { get; set; } = default!;
    public DbSet<Account> Accounts { get; set; } = default!;
    public DbSet<Collective> Collectives { get; set; } = default!;
    public DbSet<IndividualActiveCollective> IndividualActiveCollectives { get; set; } = default!;
    public DbSet<IndividualCollective> IndividualCollectives { get; set; } = default!;
    public DbSet<Individual> Individuals { get; set; } = default!;
    public DbSet<TransactionCategory> TransactionCategories { get; set; } = default!;
    public DbSet<Transaction> Transactions { get; set; } = default!;

    public AbacusContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(AccountEntityTypeConfiguration).Assembly);
    }

    IQueryable<TEntity> IAbacusContext.Set<TEntity>() where TEntity : class
    {
        return Set<TEntity>().AsQueryable();
    }
}

interface IAbacusContext
{
    IQueryable<TEntity> Set<TEntity>() where TEntity : class;
}

