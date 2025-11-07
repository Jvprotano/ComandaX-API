using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.WebAPI.GraphQL.DataLoaders;

public class GetTableByIdDataLoader(IBatchScheduler batchScheduler, DataLoaderOptions options, ITableRepository tableRepository) : BatchDataLoader<Guid, Table>(batchScheduler, options)
{
    private readonly ITableRepository _tableRepository = tableRepository;

    protected override async Task<IReadOnlyDictionary<Guid, Table>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var result = await _tableRepository.GetByIdsAsync(keys);
        return result.ToDictionary(t => t.Id);
    }
}
