using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KiriathSolutions.Tolkien.Api.Entities;

public class AccountCategory : IEntity, IIndividualCreatedEntity, ICollectiveEntity
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

    public virtual IEnumerable<Account> Accounts { get; set; } = new List<Account>();
}

public class AccountCategoryEntityTypeConfiguration : IEntityTypeConfiguration<AccountCategory>
{
    public void Configure(EntityTypeBuilder<AccountCategory> builder)
    {
        IEntity.Configure(builder);
        IIndividualCreatedEntity.Configure(builder);
        ICollectiveEntity.Configure(builder);

        builder
            .ToTable("account_categories")
            .HasKey((e) => e.Id);

        builder
            .Property((e) => e.Name)
            .HasColumnName("name");

        builder
            .Property((e) => e.CollectiveId)
            .HasColumnName("collective_id");

        builder
            .HasOne((e) => e.Collective)
            .WithMany((e) => e.AccountCategories)
            .HasForeignKey((e) => e.CollectiveId);
    }
}