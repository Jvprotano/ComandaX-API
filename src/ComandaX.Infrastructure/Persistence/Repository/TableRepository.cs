using Microsoft.EntityFrameworkCore;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.Infrastructure.Repository;

public class TableRepository(AppDbContext _context) : ITableRepository
{
    public async Task<Table> AddAsync(Table table)
    {
        var maxCode = await GetMaxCodeAsync();
        table.SetCode(maxCode + 1);

        await _context.Tables.AddAsync(table);
        return await _context.SaveChangesAsync().ContinueWith(_ => table);
    }

    public async Task<IEnumerable<Table>> GetAllAsync()
    {
        return await _context.Tables.ToListAsync();
    }

    public async Task<Table?> GetByIdAsync(Guid id)
    {
        var table = await _context.Tables.FirstOrDefaultAsync(t => t.Id == id);
        return table;
    }

    public async Task<IList<Table>> GetByIdsAsync(IReadOnlyList<Guid> ids)
    {
        return await _context.Tables.Where(t => ids.Contains(t.Id)).ToListAsync();
    }


    public async Task<int> GetMaxCodeAsync()
    {
        var maxCode = await _context.Tables.MaxAsync(t => (int?)t.Code) ?? 0;
        return maxCode;
    }

    public async Task UpdateAsync(Table table)
    {
        _context.Tables.Update(table);
        await _context.SaveChangesAsync();
    }
}
