using Microsoft.EntityFrameworkCore;
using ComandaX.Application.DTOs;
using ComandaX.Application.Extensions;
using ComandaX.Infrastructure;

namespace ComandaX.WebAPI.GraphQL.DataLoaders;

public class GetOrderByCustomerTabIdDataLoader(
    IBatchScheduler batchScheduler,
    DataLoaderOptions options,
    IDbContextFactory<AppDbContext> dbContextFactory)
    : GroupedDataLoader<Guid, OrderDto>(batchScheduler, options)
{
    protected override async Task<ILookup<Guid, OrderDto>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var items = await db.Orders
            .Where(i => i.CustomerTabId.HasValue && keys.Contains(i.CustomerTabId.Value))
            .Select(i => i.AsDto())
            .ToListAsync(cancellationToken);

        return items.ToLookup(i => i.CustomerTabId!.Value);
    }
}
