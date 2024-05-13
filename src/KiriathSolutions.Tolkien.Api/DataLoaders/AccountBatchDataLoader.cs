using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Repositories;

namespace KiriathSolutions.Tolkien.Api.DataLoaders
{
    internal sealed class AccountBatchDataLoader : BatchDataLoader<int, Account>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountBatchDataLoader(
            IUnitOfWork unitOfWork,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null)
            : base(batchScheduler, options)
        {
            _unitOfWork = unitOfWork;
        }

        protected override async Task<IReadOnlyDictionary<int, Account>> LoadBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            var accounts = await _unitOfWork.Accounts.FindByIdsAsync(keys);
            return accounts.ToDictionary(x => x.Id);
        }
    }
}