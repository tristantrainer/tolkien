using System.Collections;
using System.Linq.Expressions;
using KiriathSolutions.Tolkien.Api.Auth;
using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KiriathSolutions.Tolkien.Api.Repositories;

internal class DbRepository<TEntity> where TEntity : class, IEntity
{
    public readonly DbSet<TEntity> Entities;

    public DbRepository(DbContext dbContext)
    {
        Entities = dbContext.Set<TEntity>();
    }

    public Task<TEntity?> FindByIdAsync(int? id)
    {
        return Entities.SingleOrDefaultAsync((record) => record.Id == id);
    }

    public Task<TEntity?> FindByPublicIdAsync<TIdModel>(IEntityPublicId<TIdModel, TEntity>? publicId)
        where TIdModel : IEntityPublicId<TIdModel, TEntity>, new()
    {
        if (publicId == null)
            return Task.FromResult<TEntity?>(null);

        return Entities.SingleOrDefaultAsync((record) => record.PublicId == publicId.Value);
    }

    public Task<TEntity[]> FindByIdsAsync(IReadOnlyList<int> ids)
    {
        return Entities
            .Where((record) => ids.Contains(record.Id))
            .ToArrayAsync();
    }

    public Task DeleteAsync(TEntity entity)
    {
        Entities.Remove(entity);
        return Task.CompletedTask;
    }

    public static implicit operator QueryBuilder<TEntity>(DbRepository<TEntity> repository) => new(repository);
}