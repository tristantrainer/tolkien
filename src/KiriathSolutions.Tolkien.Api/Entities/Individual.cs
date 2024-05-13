using KiriathSolutions.Tolkien.Api.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KiriathSolutions.Tolkien.Api.Entities;

public class Individual : IEntity, IJwtBearer
{
    public int Id { get; set; }
    public Guid PublicId { get; init; }
    public DateTime Created { get; init; }
    public DateTime LastUpdated { get; set; }

    public string EmailAddress { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string? RefreshToken { get; set; } = default!;
    public DateTime? RefreshTokenExpires { get; set; }

    public virtual IndividualActiveCollective? ActiveCollective { get; set; }
    public virtual IEnumerable<IndividualCollective> IndividualCollectives { get; set; } = new List<IndividualCollective>();
}

public class IndividualEntityTypeConfiguration : IEntityTypeConfiguration<Individual>
{
    public void Configure(EntityTypeBuilder<Individual> builder)
    {
        IEntity.Configure(builder);

        builder
            .ToTable("individuals")
            .HasKey((e) => e.Id);

        builder
            .Property((e) => e.EmailAddress)
            .HasColumnName("email_address");

        builder
            .Property((e) => e.FirstName)
            .HasColumnName("first_name");

        builder
            .Property((e) => e.LastName)
            .HasColumnName("last_name");

        builder
            .Property((e) => e.PasswordHash)
            .HasColumnName("password_hash");

        builder
            .Property((e) => e.RefreshToken)
            .HasColumnName("refresh_token");

        builder
            .Property((e) => e.RefreshTokenExpires)
            .HasColumnName("refresh_token_expires");
    }
}