using Microsoft.EntityFrameworkCore;
using ComandaX.Application.DTOs;
using ComandaX.Application.Extensions;
using ComandaX.Infrastructure;

namespace ComandaX.WebAPI.GraphQL.DataLoaders;

public class GetTableByIdDataLoader(
    IBatchScheduler batchScheduler,
    DataLoaderOptions options,
    IDbContextFactory<AppDbContext> dbContextFactory)
    : BatchDataLoader<Guid, TableDto>(batchScheduler, options)
{

    protected override async Task<IReadOnlyDictionary<Guid, TableDto>> LoadBatchAsync(
        IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await db.Tables
            .Where(t => keys.Contains(t.Id))
            .Select(t => t.AsDto())
            .ToDictionaryAsync(t => t.Id, cancellationToken);
    }
}
