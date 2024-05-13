using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KiriathSolutions.Tolkien.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace KiriathSolutions.Tolkien.Api.Extensions;

public static class DbContextExtensions
{
    public static TEntity? FindByPublicId<TEntity>(this DbContext context, Guid publicId) where TEntity : class, IEntity
    {
        return context.Set<TEntity>()
            .SingleOrDefault((record) => record.PublicId == publicId);
    }

    public static Task<TEntity?> FindByPublicIdAsync<TEntity>(this DbContext context, Guid? publicId) where TEntity : class, IEntity
    {
        if (publicId == null)
            return Task.FromResult<TEntity?>(null);

        return context.Set<TEntity>()
            .SingleOrDefaultAsync((record) => record.PublicId == publicId);
    }
}