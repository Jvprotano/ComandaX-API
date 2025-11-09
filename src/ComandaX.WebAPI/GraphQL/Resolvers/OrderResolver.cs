using ComandaX.Application.DTOs;
using ComandaX.WebAPI.GraphQL.DataLoaders;

namespace ComandaX.WebAPI.GraphQL.Resolvers;

public class OrderResolver
{
    public async Task<IEnumerable<OrderProductDto>> GetOrderProductsAsync(
        [Parent] OrderDto order,
        GetOrderProductByOrderIdDataLoader loader,
        CancellationToken ct)
    {
        return await loader.LoadAsync(order.Id, ct) ?? [];
    }
}
