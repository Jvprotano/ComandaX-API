using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.WebAPI.GraphQL.DataLoaders;

public class GetProductByIdDataLoader(IBatchScheduler batchScheduler, DataLoaderOptions options, IProductRepository productRepository) : BatchDataLoader<Guid, Product>(batchScheduler, options)
{
    private readonly IProductRepository _productRepository = productRepository;

    protected override async Task<IReadOnlyDictionary<Guid, Product>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var result = await _productRepository.GetByIdsAsync(keys);
        return result.ToDictionary(p => p.Id);
    }
}

