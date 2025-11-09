using Microsoft.EntityFrameworkCore;
using ComandaX.Application.DTOs;
using ComandaX.Infrastructure;
using ComandaX.Application.Extensions;

namespace ComandaX.WebAPI.GraphQL.DataLoaders;

public class GetOrderProductByOrderIdDataLoader(
    IBatchScheduler batchScheduler,
    IDbContextFactory<AppDbContext> dbContextFactory,
    DataLoaderOptions options)
        : GroupedDataLoader<Guid, OrderProductDto>(batchScheduler, options)
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory = dbContextFactory;

    protected override async Task<ILookup<Guid, OrderProductDto>> LoadGroupedBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var items = await db.OrderProducts
            .Where(i => keys.Contains(i.OrderId))
            .Select(i => i.AsDto())
            .ToListAsync(cancellationToken);

        return items.ToLookup(i => i.OrderId);
    }
}

