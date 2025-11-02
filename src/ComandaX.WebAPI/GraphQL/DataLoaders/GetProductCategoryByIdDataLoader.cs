using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.WebAPI.GraphQL.DataLoaders;

public class GetProductCategoryByIdDataLoader : BatchDataLoader<Guid, ProductCategory>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    public GetProductCategoryByIdDataLoader(IBatchScheduler batchScheduler, DataLoaderOptions options, IProductCategoryRepository productCategoryRepository) : base(batchScheduler, options)
    {
        _productCategoryRepository = productCategoryRepository;
    }

    protected override async Task<IReadOnlyDictionary<Guid, ProductCategory>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var result = await _productCategoryRepository.GetByIdsAsync(keys);
        return result.ToDictionary(t => t.Id);
    }
}
