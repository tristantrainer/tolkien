using KiriathSolutions.Tolkien.Api.Auth;
using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KiriathSolutions.Tolkien.Api.Repositories;

internal sealed class CollectiveRepository : DbRepository<Collective>
{
    public CollectiveRepository(DbContext dbContext) : base(dbContext) { }

    public async Task<int> GetCollectiveId(Guid publicId)
    {
        var collective = await Entities
            .FirstAsync((entity) => entity.PublicId == publicId);

        return collective.Id;
    }

    public async Task<Collective[]> GetAllForUser(ITolkienUser user)
    {
        return await Entities
            .Include((record) => record.IndividualCollectives)
            .Where((record) => record.IndividualCollectives.Any((map) => map.IndividualId == user.IndividualId))
            .ToArrayAsync();
    }
}
