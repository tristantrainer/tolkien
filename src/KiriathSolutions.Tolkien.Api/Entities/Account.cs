using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KiriathSolutions.Tolkien.Api.Entities;

public class Account : IEntity, IIndividualCreatedEntity, ICollectiveEntity
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

    public int CategoryId { get; set; }
    public virtual AccountCategory? Category { get; set; }

    public virtual IEnumerable<AccountHistory> HistoryItems { get; set; } = new List<AccountHistory>();
    public virtual IEnumerable<Transaction> Transactions { get; set; } = new List<Transaction>();
}

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        IEntity.Configure(builder);
        IIndividualCreatedEntity.Configure(builder);
        ICollectiveEntity.Configure(builder);

        builder
            .ToTable("accounts")
            .HasKey((e) => e.Id);

        builder
            .Property((e) => e.Name)
            .HasColumnName("name");

        builder
            .Property((e) => e.CategoryId)
            .HasColumnName("category_id");

        builder
            .HasOne((e) => e.Category)
            .WithMany((e) => e.Accounts)
            .HasForeignKey((e) => e.CategoryId);

        builder
            .HasOne((e) => e.Collective)
            .WithMany((e) => e.Accounts)
            .HasForeignKey((e) => e.CollectiveId);
    }
}