using KiriathSolutions.Tolkien.Api.Entities;

namespace KiriathSolutions.Tolkien.Api.Models;

public struct IndividualId : IEntityPublicId<IndividualId, Individual>
{
    public Guid Value { get; init; }
}

