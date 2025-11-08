using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.WebAPI.GraphQL.DataLoaders;

public class GetOrderByIdDataLoader(IBatchScheduler batchScheduler, DataLoaderOptions options, IOrderRepository orderRepository) : BatchDataLoader<Guid, Order>(batchScheduler, options)
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    protected override async Task<IReadOnlyDictionary<Guid, Order>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var result = await _orderRepository.GetByIdsAsync(keys);
        return result.ToDictionary(p => p.Id);
    }
}
