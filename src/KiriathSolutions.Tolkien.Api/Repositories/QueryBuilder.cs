using System.Linq.Expressions;
using KiriathSolutions.Tolkien.Api.Auth;
using KiriathSolutions.Tolkien.Api.Entities;

namespace KiriathSolutions.Tolkien.Api.Repositories;

internal sealed class QueryBuilder<T> where T : class, IEntity
{
    private DbRepository<T> _repository;
    private List<Expression<Func<T, bool>>> _filters = new();


    public QueryBuilder(DbRepository<T> repository)
    {
        _repository = repository;
    }

    public void AddFilter(Expression<Func<T, bool>> filter)
    {
        _filters.Add(filter);
    }
}

internal static class QueryBuilderExtensions
{
    public static QueryBuilder<T> ForUser<T>(this QueryBuilder<T> builder, ITolkienUser user)
        where T : class, IEntity, ICollectiveEntity
    {
        builder.AddFilter((entity) => user.CollectiveIds.Contains(entity.CollectiveId));
        return builder;
    }
}