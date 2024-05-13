using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KiriathSolutions.Tolkien.Api.Entities;

public class TransactionCategory : IEntity, IIndividualCreatedEntity, ICollectiveEntity
{
    public int Id { get; set; }
    public Guid PublicId { get; init; }

    public DateTime Created { get; init; }
    public int CreatedBy { get; init; }
    public DateTime LastUpdated { get; set; }
    public int UpdatedBy { get; set; }

    public string Name { get; set; } = default!;

    public int CollectiveId { get; set; }
    public virtual Collective? Collective { get; set; }

    public virtual IEnumerable<Transaction> Transactions { get; set; } = new List<Transaction>();
}

public class TransactionCategoryEntityTypeConfiguration : IEntityTypeConfiguration<TransactionCategory>
{
    public void Configure(EntityTypeBuilder<TransactionCategory> builder)
    {
        IEntity.Configure(builder);
        IIndividualCreatedEntity.Configure(builder);
        ICollectiveEntity.Configure(builder);

        builder
            .ToTable("transaction_categories")
            .HasKey((e) => e.Id);

        builder
            .Property((e) => e.Name)
            .HasColumnName("name");

        builder
            .HasOne((e) => e.Collective)
            .WithMany((e) => e.TransactionCategories)
            .HasForeignKey((e) => e.CollectiveId);
    }
}