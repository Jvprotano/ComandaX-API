using ComandaX.Application.DTOs;
using ComandaX.WebAPI.GraphQL.DataLoaders;

namespace ComandaX.WebAPI.GraphQL.Resolvers;

public class OrderProductResolvers
{
    public async Task<ProductDto?> GetProduct(
        [Parent] OrderProductDto orderProduct,
        GetProductByIdDataLoader dataLoader,
        CancellationToken cancellationToken)
    {
        return await dataLoader.LoadAsync(orderProduct.ProductId, cancellationToken);
    }
}
