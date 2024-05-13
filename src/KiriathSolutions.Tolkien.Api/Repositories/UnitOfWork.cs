using KiriathSolutions.Tolkien.Api.Auth;
using KiriathSolutions.Tolkien.Api.Contexts;
using KiriathSolutions.Tolkien.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace KiriathSolutions.Tolkien.Api.Repositories;

internal sealed class UnitOfWork : IUnitOfWork
{
    private AbacusContext _dbContext;
    private readonly IDbContextFactory<AbacusContext> _dbContextFactory;

    private readonly Dictionary<Type, object> _repositories = new();

    private AccountRepository? _accounts;
    private AccountCategoryRepository? _accountCategories;
    private CollectiveRepository? _collectives;
    private IndividualActiveCollectiveRepository? _individualActiveCollectives;
    private IndividualCollectiveRepository? _individualCollectives;
    private IndividualRepository? _individuals;
    private TransactionRepository? _transactions;
    private TransactionCategoryRepository? _transactionCategories;

    public AccountRepository Accounts => _accounts ??= new AccountRepository(_dbContext);
    public AccountCategoryRepository AccountCategories => _accountCategories ??= new AccountCategoryRepository(_dbContext);
    public CollectiveRepository Collectives => _collectives ??= new CollectiveRepository(_dbContext);
    public IndividualActiveCollectiveRepository IndividualActiveCollectives => _individualActiveCollectives ??= new IndividualActiveCollectiveRepository(_dbContext);
    public IndividualCollectiveRepository IndividualCollectives => _individualCollectives ??= new IndividualCollectiveRepository(_dbContext);
    public IndividualRepository Individuals => _individuals ??= new IndividualRepository(_dbContext);
    public TransactionRepository Transactions => _transactions ??= new TransactionRepository(_dbContext);
    public TransactionCategoryRepository TransactionCategories => _transactionCategories ??= new TransactionCategoryRepository(_dbContext);

    public UnitOfWork(IDbContextFactory<AbacusContext> contextFactory)
    {
        _dbContext = contextFactory.CreateDbContext();
        _dbContextFactory = contextFactory;

        _repositories.Add(typeof(Collective), Collectives);
    }

    public int SaveChanges() => _dbContext.SaveChanges();
    public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    public async ValueTask AddAsync(object entity) => await _dbContext.AddAsync(entity);

    #region Disposable Pattern

    private bool _disposed = false;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _dbContext.Dispose();
        }

        // Release unmanaged resources - Add destructor

        _disposed = true;
    }

    #endregion
}

internal interface IUnitOfWork : IDisposable
{
    AccountRepository Accounts { get; }
    AccountCategoryRepository AccountCategories { get; }
    CollectiveRepository Collectives { get; }
    IndividualActiveCollectiveRepository IndividualActiveCollectives { get; }
    IndividualCollectiveRepository IndividualCollectives { get; }
    IndividualRepository Individuals { get; }
    TransactionRepository Transactions { get; }
    TransactionCategoryRepository TransactionCategories { get; }
    ValueTask AddAsync(object entity);
    
    int SaveChanges();
    Task<int> SaveChangesAsync();
}
