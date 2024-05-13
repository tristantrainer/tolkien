using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KiriathSolutions.Tolkien.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace KiriathSolutions.Tolkien.Api.Repositories;

public class IndividualActiveCollectiveRepository
{
    protected readonly DbSet<IndividualActiveCollective> Entities;

    public IndividualActiveCollectiveRepository(DbContext dbContext)
    {
        Entities = dbContext.Set<IndividualActiveCollective>();
    }

    public async Task<IndividualActiveCollective?> GetActiveCollective(int individualId)
    {
        return await Entities
            .FirstOrDefaultAsync((record) => record.IndividualId == individualId);
    }
}
