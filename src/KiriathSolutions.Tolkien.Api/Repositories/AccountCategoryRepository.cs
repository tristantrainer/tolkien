using KiriathSolutions.Tolkien.Api.Auth;
using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KiriathSolutions.Tolkien.Api.Repositories;

internal sealed class AccountCategoryRepository : DbRepository<AccountCategory>
{
    public AccountCategoryRepository(DbContext dbContext) : base(dbContext) { }

    public async Task<AccountCategory[]> GetAllForCollectiveAsync(int collectiveId)
    {
        return await Entities
            .Where((record) => record.CollectiveId == collectiveId)
            .ToArrayAsync();
    }
}
