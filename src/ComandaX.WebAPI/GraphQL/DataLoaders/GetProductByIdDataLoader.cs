using Microsoft.EntityFrameworkCore;
using ComandaX.Application.DTOs;
using ComandaX.Application.Extensions;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using ComandaX.Infrastructure;

namespace ComandaX.WebAPI.GraphQL.DataLoaders;

public class GetProductByIdDataLoader(
    IBatchScheduler batchScheduler,
    DataLoaderOptions options,
    IDbContextFactory<AppDbContext> dbContextFactory) : BatchDataLoader<Guid, ProductDto>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, ProductDto>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await db.Products
            .Where(p => keys.Contains(p.Id))
            .Select(p => p.AsDto())
            .ToDictionaryAsync(p => p.Id, cancellationToken);
    }
}

