using KiriathSolutions.Tolkien.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace KiriathSolutions.Tolkien.Api.Contexts;

public class AuthContext : DbContext
{
    public DbSet<Individual> Individuals { get; set; } = default!;
    public DbSet<IndividualCollective> IndividualCollectives { get; set; } = default!;
    public DbSet<Collective> Collectives { get; set; } = default!;

    public AuthContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(AccountEntityTypeConfiguration).Assembly);
    }
}
