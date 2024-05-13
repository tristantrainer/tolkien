using KiriathSolutions.Tolkien.Api.Entities;

namespace KiriathSolutions.Tolkien.Api.Models;

public struct CollectiveId : IEntityPublicId<CollectiveId, Collective>
{
    public Guid Value { get; init; }
}

