using ComandaX.Domain.Entities;

namespace ComandaX.Application.Interfaces;

public interface ITableRepository
{
    Task<Table> AddAsync(Table table);
    Task<Table?> GetByIdAsync(Guid id);
    Task<IQueryable<Table>> GetAllAsync();
    Task UpdateAsync(Table table);
    Task<IList<Table>> GetByIdsAsync(IReadOnlyList<Guid> ids);
    Task SetAsBusyAsync(Guid id);
}
