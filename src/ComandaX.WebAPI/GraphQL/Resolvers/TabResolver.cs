using ComandaX.Application.DTOs;
using ComandaX.Domain.Entities;
using ComandaX.WebAPI.GraphQL.DataLoaders;

namespace ComandaX.WebAPI.GraphQL.Resolvers;

public class TabResolvers
{
    public async Task<TableDto?> GetTable(
        [Parent] TabDto tab,
        GetTableByIdDataLoader dataLoader,
        CancellationToken cancellationToken)
    {
        if (tab.TableId == null)
            return null;

        var table = await dataLoader.LoadAsync(tab.TableId.Value, cancellationToken);

        if (table == null)
            return null;

        return new(table.Id, table.Code, table.Status);
    }
}
