using KiriathSolutions.Tolkien.Api.Entities;

namespace KiriathSolutions.Tolkien.Api.Models;

public interface IEntityPublicId<TIdModel, TEntity>
    where TIdModel : IEntityPublicId<TIdModel, TEntity>, new()
    where TEntity : IEntity
{
    Guid Value { get; init; }

    virtual static implicit operator Guid(TIdModel id) => id.Value;
    virtual static implicit operator TIdModel(Guid id) => new() { Value = id };
    virtual string ToString(string format) => Value.ToString(format);
}

public interface IEntityId<TIdModel, TEntity>
    where TIdModel : struct, IEntityId<TIdModel, TEntity>
    where TEntity : IEntity
{
    int Value { get; init; }

    virtual static implicit operator int(TIdModel id) => id.Value;
    virtual static implicit operator TIdModel(int id) => new() { Value = id };
}