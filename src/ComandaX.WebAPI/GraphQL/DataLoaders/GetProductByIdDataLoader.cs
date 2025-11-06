using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.WebAPI.GraphQL.DataLoaders;

public class GetProductByIdDataLoader : BatchDataLoader<Guid, Product>
{
    private readonly IProductRepository _productRepository;
    public GetProductByIdDataLoader(IBatchScheduler batchScheduler, DataLoaderOptions options, IProductRepository productRepository) : base(batchScheduler, options)
    {
        _productRepository = productRepository;
    }

    protected override async Task<IReadOnlyDictionary<Guid, Product>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var result = await _productRepository.GetByIdsAsync(keys);
        return result.ToDictionary(p => p.Id);
    }
}

