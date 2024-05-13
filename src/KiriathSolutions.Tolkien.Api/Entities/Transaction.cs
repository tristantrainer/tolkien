using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KiriathSolutions.Tolkien.Api.Entities;

public class Transaction : IEntity, IIndividualCreatedEntity, ICollectiveEntity
{
    public int Id { get; set; }
    public Guid PublicId { get; init; }

    public DateTime Created { get; init; }
    public int CreatedBy { get; init; }
    public DateTime LastUpdated { get; set; }
    public int UpdatedBy { get; set; }

    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public string? Recurrance { get; set; }

    public int? AccountId { get; set; }
    public virtual Account? Account { get; set; }

    public int CategoryId { get; set; }
    public virtual TransactionCategory? Category { get; set; }

    public int CollectiveId { get; set; }
    public virtual Collective? Collective { get; set; }
}

public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        IEntity.Configure(builder);
        IIndividualCreatedEntity.Configure(builder);
        ICollectiveEntity.Configure(builder);

        builder
            .ToTable("transactions")
            .HasKey((e) => e.Id);

        builder
            .Property((e) => e.AccountId)
            .HasColumnName("account_id");

        builder
            .Property((e) => e.Amount)
            .HasColumnName("amount");

        builder
            .Property((e) => e.CategoryId)
            .HasColumnName("category_id");

        builder
            .Property((e) => e.Date)
            .HasColumnName("date");

        builder
            .Property((e) => e.Description)
            .HasColumnName("description");

        builder
            .Property((e) => e.Recurrance)
            .HasColumnName("recurrance");

        builder
            .HasOne((e) => e.Account)
            .WithMany((e) => e.Transactions)
            .HasForeignKey((e) => e.AccountId);

        builder
            .HasOne((e) => e.Category)
            .WithMany((e) => e.Transactions)
            .HasForeignKey((e) => e.CategoryId);

        builder
            .HasOne((e) => e.Collective)
            .WithMany((e) => e.Transactions)
            .HasForeignKey((e) => e.CollectiveId);
    }
}