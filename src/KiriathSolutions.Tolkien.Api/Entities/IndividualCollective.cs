using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KiriathSolutions.Tolkien.Api.Entities;

public class IndividualCollective
{
    public int IndividualId { get; init; }
    public virtual Individual? Individual { get; set; }

    public int CollectiveId { get; init; }
    public virtual Collective? Collective { get; set; }
}

public class IndividualCollectiveEntityTypeConfiguration : IEntityTypeConfiguration<IndividualCollective>
{
    public void Configure(EntityTypeBuilder<IndividualCollective> builder)
    {
        builder
            .ToTable("individual_collectives")
            .HasKey((e) => new { e.IndividualId, e.CollectiveId });

        builder
            .Property((e) => e.CollectiveId)
            .HasColumnName("collective_id");

        builder
            .Property((e) => e.IndividualId)
            .HasColumnName("individual_id");

        builder
            .HasOne((e) => e.Individual)
            .WithMany((e) => e.IndividualCollectives)
            .HasForeignKey((e) => e.IndividualId);

        builder
            .HasOne((e) => e.Collective)
            .WithMany((e) => e.IndividualCollectives)
            .HasForeignKey((e) => e.CollectiveId);
    }
}
