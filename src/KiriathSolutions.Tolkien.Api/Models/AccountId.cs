using KiriathSolutions.Tolkien.Api.Entities;

namespace KiriathSolutions.Tolkien.Api.Models;

public readonly struct AccountPublicId : IEntityPublicId<AccountPublicId, Account>
{
    public Guid Value { get; init; }
}

public readonly struct AccountId : IEntityId<AccountId, Account>
{
    public int Value { get; init; }
}