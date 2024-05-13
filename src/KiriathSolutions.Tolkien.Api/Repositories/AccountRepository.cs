using KiriathSolutions.Tolkien.Api.Contexts;
using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KiriathSolutions.Tolkien.Api.Repositories;

internal sealed class AccountRepository : DbRepository<Account>
{
    private AbacusContext _abacusContext;

    public AccountRepository(AbacusContext dbContext) : base(dbContext)
    {
        _abacusContext = dbContext;
    }

    public async Task<Account[]> GetAllForCollectiveAsync(int collectiveId)
    {
        return await Entities
            .Where((record) => record.CollectiveId == collectiveId)
            .ToArrayAsync();
    }
}
