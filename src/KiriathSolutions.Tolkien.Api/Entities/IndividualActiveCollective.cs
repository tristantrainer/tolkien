using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KiriathSolutions.Tolkien.Api.Entities;

public class IndividualActiveCollective
{
    public int IndividualId { get; init; }
    public virtual Individual? Individual { get; set; }

    public int CollectiveId { get; set; }
    public virtual Collective? Collective { get; set; }
}


public class IndividualActiveCollectiveEntityTypeConfiguration : IEntityTypeConfiguration<IndividualActiveCollective>
{
    public void Configure(EntityTypeBuilder<IndividualActiveCollective> builder)
    {
        builder
            .ToTable("individual_active_collectives")
            .HasKey((e) => new { e.IndividualId });

        builder
            .Property((e) => e.CollectiveId)
            .HasColumnName("collective_id");

        builder
            .Property((e) => e.IndividualId)
            .HasColumnName("individual_id");

        builder
            .HasOne((e) => e.Individual)
            .WithOne((e) => e.ActiveCollective)
            .HasForeignKey<IndividualActiveCollective>((e) => e.IndividualId);
    }
}
