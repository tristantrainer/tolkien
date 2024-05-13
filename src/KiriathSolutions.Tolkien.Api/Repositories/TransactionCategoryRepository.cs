using Microsoft.EntityFrameworkCore;
using KiriathSolutions.Tolkien.Api.Entities;

namespace KiriathSolutions.Tolkien.Api.Repositories;

internal sealed class TransactionCategoryRepository : DbRepository<TransactionCategory>
{
    public TransactionCategoryRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
