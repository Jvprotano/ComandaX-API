using ComandaX.Application.DTOs;

namespace ComandaX.WebAPI.GraphQL.DataLoaders;

public class TableDataLoader : BatchDataLoader<Guid, TableDto>
{
    public TableDataLoader(IBatchScheduler batchScheduler, DataLoaderOptions options) : base(batchScheduler, options)
    {
    }

    protected override Task<IReadOnlyDictionary<Guid, TableDto>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
