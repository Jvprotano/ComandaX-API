using ComandaX.Domain.Entities;

namespace ComandaX.Application.Interfaces;

public interface IOrderProductRepository
{
    Task<IList<OrderProduct>> GetByIdsAsync(IReadOnlyList<Guid> ids);
}
