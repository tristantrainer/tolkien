using System.Linq;
using KiriathSolutions.Tolkien.Api.Auth;
using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Extensions;
using KiriathSolutions.Tolkien.Api.Models;
using KiriathSolutions.Tolkien.Api.Repositories;

namespace KiriathSolutions.Tolkien.Api.Services;
internal sealed class AccountService
{
    private IUnitOfWork _unitOfWork;
    private ITolkienUser _user;

    public AccountService(ITolkienUser user, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _user = user;
    }

    public async Task<EntityMutationResult<Account>> UpdateAccountBalance(AccountPublicId accountId, DateOnly date, decimal balance)
    {
        var account = await GetAccount(accountId);

        if (account is { Value: null } or { Access: EntityAccess.Missing or EntityAccess.Denied })
            return EntityMutationResult.Deny<Account>();





        return EntityMutationResult.Allow(account.Value); ;
    }

    public async Task<EntityQueryResult<Account>> GetAccount(AccountPublicId accountId)
    {
        var account = await _unitOfWork
            .Accounts
            .FindByPublicIdAsync(accountId);

        if (account is null)
            return EntityQueryResult.Missing<Account>();

        if (!_user.CollectiveIds.Contains(account.CollectiveId))
            return EntityQueryResult.Deny<Account>();

        return EntityQueryResult.Allow(account);
    }
}

internal static class EntityMutationResult
{
    public static EntityMutationResult<T> Deny<T>() => new(default, EntityAccess.Denied);
    public static EntityMutationResult<T> Allow<T>(T entity) => new(entity, EntityAccess.Denied);
}

internal static class EntityQueryResult
{
    public static EntityQueryResult<T> Deny<T>() => new(default, EntityAccess.Denied);
    public static EntityQueryResult<T> Missing<T>() => new(default, EntityAccess.Missing);
    public static EntityQueryResult<T> Allow<T>(T entity) => new(entity, EntityAccess.Given);
}

internal record EntityQueryResult<TEntity>(TEntity? Value, EntityAccess Access);
internal record EntityMutationResult<TEntity>(TEntity? Value, EntityAccess Result);

internal enum EntityAccess
{
    Denied,
    Given,
    Missing,
}