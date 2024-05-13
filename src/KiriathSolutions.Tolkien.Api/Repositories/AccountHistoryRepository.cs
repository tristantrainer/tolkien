using KiriathSolutions.Tolkien.Api.Auth;
using KiriathSolutions.Tolkien.Api.Contexts;
using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KiriathSolutions.Tolkien.Api.Repositories;

public class AccountHistoryRepository
{
    private AbacusContext _abacusContext;

    public AccountHistoryRepository(AbacusContext dbContext)
    {
        _abacusContext = dbContext;
    }

    public async Task<AccountHistory[]> GetAccountHistoryItems(AccountId accountId)
    {
        return await _abacusContext
            .AccountHistories
            .Where((item) => item.AccountId == accountId.Value)
            .ToArrayAsync();
    }

    public async Task<AccountHistory?> GetAccountHistoryItem(AccountId accountId, DateOnly date)
    {
        return await _abacusContext
            .AccountHistories
            .FirstOrDefaultAsync((record) => record.AccountId == accountId.Value && record.Date == date);
    }

    public async Task<DbUpdateResult> UpdateAccountHistory(Individual individual, AccountId accountId, DateOnly date, decimal balance, DateTime? updatedAt = null)
    {
        var historyItem = await _abacusContext
            .AccountHistories
            .FirstOrDefaultAsync((item) => item.AccountId == accountId.Value && item.Date.Equals(date));

        if (historyItem is null)
            return DbUpdateResult.NotFound();

        historyItem.Balance = balance;
        historyItem.UpdatedBy = individual.Id;
        historyItem.LastUpdated = updatedAt ?? DateTime.UtcNow;

        await _abacusContext.SaveChangesAsync();
        return DbUpdateResult.Success();
    }

    public async Task<DbCreateResult> AddAccountHistory(Individual individual, AccountId accountId, decimal balance, DateOnly date, DateTime? createdAt = null)
    {
        var historyItem = await _abacusContext
            .AccountHistories
            .FirstOrDefaultAsync((item) => item.AccountId == accountId.Value && item.Date.Equals(date));

        if (historyItem is not null)
            return DbCreateResult.DuplicateFound();

        var newAccountHistoryItem = AccountHistoryBuilder
            .CreateAs(individual, createdAt)
            .ForAccount(accountId)
            .WithBalance(balance)
            .OnDate(date)
            .Build();

        await _abacusContext
            .AccountHistories
            .AddAsync(newAccountHistoryItem);

        await _abacusContext.SaveChangesAsync();
        return DbCreateResult.Success();
    }
}

public record DbCreateResult(CreateResult Result)
{
    public static DbCreateResult Success() => new(CreateResult.Success);
    public static DbCreateResult DuplicateFound() => new(CreateResult.DuplicateFound);

}

public record DbUpdateResult(UpdateResult Result)
{
    public static DbUpdateResult Success() => new(UpdateResult.Success);
    public static DbUpdateResult NotFound() => new(UpdateResult.NotFound);
}

public enum UpdateResult
{
    Failed,
    Success,
    NotFound,
}

public enum CreateResult
{
    Failed,
    Success,
    DuplicateFound,
}