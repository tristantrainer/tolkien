using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KiriathSolutions.Tolkien.Api.Entities;

public class Collective : IEntity, IIndividualCreatedEntity
{
    public int Id { get; set; }
    public Guid PublicId { get; init; }

    public string Name { get; set; } = default!;

    public DateTime Created { get; init; }
    public int CreatedBy { get; init; }
    public DateTime LastUpdated { get; set; }
    public int UpdatedBy { get; set; }

    public virtual IEnumerable<Account> Accounts { get; set; } = new List<Account>();
    public virtual IEnumerable<AccountCategory> AccountCategories { get; set; } = new List<AccountCategory>();
    public virtual IEnumerable<IndividualCollective> IndividualCollectives { get; set; } = new List<IndividualCollective>();
    public virtual IEnumerable<Transaction> Transactions { get; set; } = new List<Transaction>();
    public virtual IEnumerable<TransactionCategory> TransactionCategories { get; set; } = new List<TransactionCategory>();
}

public class CollectiveEntityTypeConfiguration : IEntityTypeConfiguration<Collective>
{
    public void Configure(EntityTypeBuilder<Collective> builder)
    {
        builder
            .ToTable("collectives")
            .HasKey((e) => e.Id);

        builder
            .Property((e) => e.Name)
            .HasColumnName("name");

        IEntity.Configure(builder);
        IIndividualCreatedEntity.Configure(builder);
    }
}