using KiriathSolutions.Tolkien.Api.Auth;
using KiriathSolutions.Tolkien.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace KiriathSolutions.Tolkien.Api.Repositories;

public class IndividualCollectiveRepository
{
    protected readonly DbSet<IndividualCollective> Entities;

    public IndividualCollectiveRepository(DbContext dbContext)
    {
        Entities = dbContext.Set<IndividualCollective>();
    }

    public async Task<bool> IsIndividualInCollectiveAsync(int individualId, int collectiveId)
    {
        return await Entities
            .AnyAsync((record) => record.IndividualId == individualId && record.CollectiveId == collectiveId);
    }

    public async Task<int[]> GetIndividualCollectives(int individualId)
    {
        return await Entities
            .Where((record) => record.IndividualId == individualId)
            .Select((record) => record.CollectiveId)
            .ToArrayAsync();
    }
}
