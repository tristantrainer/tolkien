using KiriathSolutions.Tolkien.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace KiriathSolutions.Tolkien.Api.Repositories;

internal class IndividualRepository : DbRepository<Individual>
{
    public IndividualRepository(DbContext dbContext) : base(dbContext) { }
}
