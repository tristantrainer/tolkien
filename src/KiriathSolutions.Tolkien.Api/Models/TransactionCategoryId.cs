using KiriathSolutions.Tolkien.Api.Entities;

namespace KiriathSolutions.Tolkien.Api.Models;

public struct TransactionCategoryId : IEntityPublicId<TransactionCategoryId, TransactionCategory>
{
    public Guid Value { get; init; }
}