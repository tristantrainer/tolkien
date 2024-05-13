using KiriathSolutions.Tolkien.Api.Entities;

namespace KiriathSolutions.Tolkien.Api.Models;

public struct AccountHistoryId : IEntityPublicId<AccountHistoryId, AccountHistory>
{
    public Guid Value { get; init; }
}