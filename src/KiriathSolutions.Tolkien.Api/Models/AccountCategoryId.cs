using KiriathSolutions.Tolkien.Api.Entities;

namespace KiriathSolutions.Tolkien.Api.Models;

public struct AccountCategoryId : IEntityPublicId<AccountCategoryId, AccountCategory>
{
    public Guid Value { get; init; }
}