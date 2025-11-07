using ComandaX.Application.DTOs;

namespace ComandaX.WebAPI.GraphQL.DataLoaders;

public class TableDataLoader(IBatchScheduler batchScheduler, DataLoaderOptions options) : BatchDataLoader<Guid, TableDto>(batchScheduler, options)
{
    protected override Task<IReadOnlyDictionary<Guid, TableDto>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
