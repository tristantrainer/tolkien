using KiriathSolutions.Tolkien.Api.Contexts;
using KiriathSolutions.Tolkien.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace KiriathSolutions.Tolkien.Api.Repositories;

internal sealed class TransactionRepository : DbRepository<Transaction>
{
    public TransactionRepository(AbacusContext dbContext) : base(dbContext) { }

    public async Task<Transaction[]> GetAllForCollectiveAsync(int collectiveId)
    {
        return await Entities
            .Where((record) => record.CollectiveId == collectiveId)
            .ToArrayAsync();
    }
}
