using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KiriathSolutions.Tolkien.Api.Entities;

public interface IEntity
{
    Guid PublicId { get; init; }
    int Id { get; set; }
    DateTime Created { get; init; }
    DateTime LastUpdated { get; set; }

    static void Configure<TEntity>(EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IEntity
    {
        builder
            .Property((e) => e.Id)
            .HasColumnName("id");

        builder
            .Property((e) => e.PublicId)
            .HasColumnName("public_id");

        builder
            .Property((e) => e.Created)
            .HasColumnName("created");

        builder
            .Property((e) => e.LastUpdated)
            .HasColumnName("last_updated");
    }
}

public interface IIndividualCreatedEntity
{
    int CreatedBy { get; init; }
    int UpdatedBy { get; set; }

    static void Configure<TEntity>(EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IIndividualCreatedEntity
    {
        builder
            .Property((e) => e.CreatedBy)
            .HasColumnName("created_by");

        builder
            .Property((e) => e.UpdatedBy)
            .HasColumnName("updated_by");
    }
}

public interface ICollectiveEntity
{
    int CollectiveId { get; set; }

    static void Configure<TEntity>(EntityTypeBuilder<TEntity> builder)
        where TEntity : class, ICollectiveEntity
    {
        builder
            .Property((e) => e.CollectiveId)
            .HasColumnName("collective_id");
    }
}

public class UserCreatedEntity : IEntity, IIndividualCreatedEntity
{
    #region IUserEntity Properties

    public int Id { get; set; }
    public Guid PublicId { get; init; }
    public DateTime Created { get; init; }
    public int CreatedBy { get; init; }
    public DateTime LastUpdated { get; set; }
    public int UpdatedBy { get; set; }

    #endregion

    public static void Configure<TEntity>(EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IIndividualCreatedEntity, IEntity
    {
        builder
            .Property((e) => e.Id)
            .HasColumnName("id");

        builder
            .Property((e) => e.PublicId)
            .HasColumnName("public_id");

        builder
            .Property((e) => e.Created)
            .HasColumnName("created");

        builder
            .Property((e) => e.CreatedBy)
            .HasColumnName("created_by");

        builder
            .Property((e) => e.LastUpdated)
            .HasColumnName("last_updated");

        builder
            .Property((e) => e.UpdatedBy)
            .HasColumnName("updated_by");
    }
}