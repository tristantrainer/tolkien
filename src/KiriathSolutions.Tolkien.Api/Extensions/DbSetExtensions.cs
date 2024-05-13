using KiriathSolutions.Tolkien.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace KiriathSolutions.Tolkien.Api.Extensions
{
    public static class DbSetExtensions
    {
        public static TEntity Find<TEntity>(this DbSet<TEntity> dbSet, Guid publicId) where TEntity : class, IEntity
        {
            return dbSet.Single((record) => record.PublicId == publicId);
        }

        public static Task<TEntity> FindAsync<TEntity>(this DbSet<TEntity> dbSet, Guid publicId) where TEntity : class, IEntity
        {
            return dbSet.SingleAsync((record) => record.PublicId == publicId);
        }
    }
}