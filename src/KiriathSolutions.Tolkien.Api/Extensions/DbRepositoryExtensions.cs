using KiriathSolutions.Tolkien.Api.Auth;
using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Models;
using KiriathSolutions.Tolkien.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KiriathSolutions.Tolkien.Api.Extensions;

internal static class BaseRepositoryExtensions
{
    public static async Task<TEntity?> FindByIdAsync<TIdModel, TEntity>(this DbRepository<TEntity> repository, IEntityPublicId<TIdModel, TEntity> entityId, ITolkienUser user)
        where TIdModel : IEntityPublicId<TIdModel, TEntity>, new()
        where TEntity : class, IEntity, ICollectiveEntity
    {
        return await repository.Entities
            .FirstOrDefaultAsync((entity) => entity.PublicId == entityId.Value && user.CollectiveIds.Contains(entity.CollectiveId));
    }

    public static async Task<Collective?> FindByIdAsync(this DbRepository<Collective> repository, CollectiveId collectiveId, ITolkienUser user)
    {
        return await repository.Entities
            .FirstOrDefaultAsync((entity) => entity.PublicId == collectiveId.Value && user.CollectiveIds.Contains(entity.Id));
    }

    public static async Task<TEntity[]> GetAllForCollectiveAsync<TEntity>(this DbRepository<TEntity> repository, int collectiveId, ITolkienUser user)
        where TEntity : class, IEntity, ICollectiveEntity
    {
        if (!user.CollectiveIds.Contains(collectiveId))
            return Array.Empty<TEntity>();

        return await repository
            .Entities
            .Where((entity) => entity.CollectiveId == collectiveId)
            .ToArrayAsync();
    }
}
