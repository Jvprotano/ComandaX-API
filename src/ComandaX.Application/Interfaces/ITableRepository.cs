using ComandaX.Domain.Entities;

namespace ComandaX.Application.Interfaces;

public interface ITableRepository
{
    Task<Table> AddAsync(Table table);
    Task<Table?> GetByIdAsync(Guid id);
    Task<IEnumerable<Table>> GetAllAsync();
    Task<int> GetMaxCodeAsync();
    Task UpdateAsync(Table table);
}
