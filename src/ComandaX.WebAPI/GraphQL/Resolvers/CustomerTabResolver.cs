using ComandaX.Application.DTOs;
using ComandaX.Application.Extensions;
using ComandaX.WebAPI.GraphQL.DataLoaders;

namespace ComandaX.WebAPI.GraphQL.Resolvers;

public class CustomerTabResolvers
{
    public async Task<TableDto?> GetTable(
        [Parent] CustomerTabDto tab,
        GetTableByIdDataLoader dataLoader,
        CancellationToken cancellationToken)
    {
        if (tab.TableId == null)
            return null;

        var table = await dataLoader.LoadAsync(tab.TableId.Value, cancellationToken);

        if (table == null)
            return null;

        return new(table.Id, table.Number, table.Status);
    }

    public async Task<IEnumerable<OrderDto>> GetOrders(
        [Parent] CustomerTabDto tab,
        GetOrderByIdDataLoader dataLoader,
        CancellationToken cancellationToken)
    {
        if (tab.OrderIds == null || !tab.OrderIds.Any())
            return [];

        var orders = await dataLoader.LoadAsync(tab.OrderIds.ToArray(), cancellationToken);
        return orders.Where(o => o != null).Select(o => o!.AsDto());
    }
}
