using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.WebAPI.GraphQL.DataLoaders;

public class GetTableByIdDataLoader : BatchDataLoader<Guid, Table>
{
    private readonly ITableRepository _tableRepository;
    public GetTableByIdDataLoader(IBatchScheduler batchScheduler, DataLoaderOptions options, ITableRepository tableRepository) : base(batchScheduler, options)
    {
        _tableRepository = tableRepository;
    }

    protected override async Task<IReadOnlyDictionary<Guid, Table>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var result = await _tableRepository.GetByIdsAsync(keys);
        return result.ToDictionary(t => t.Id);
    }
}
