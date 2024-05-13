using KiriathSolutions.Tolkien.Api.Entities;

namespace KiriathSolutions.Tolkien.Api.Models;

public struct TransactionId : IEntityPublicId<TransactionId, Transaction>
{
    public Guid Value { get; init; }
}