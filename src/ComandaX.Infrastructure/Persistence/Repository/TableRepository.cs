using Microsoft.EntityFrameworkCore;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.Infrastructure.Persistence.Repository;

public class TableRepository(AppDbContext _context) : ITableRepository
{
    public async Task<Table> AddAsync(Table table)
    {
        await _context.Tables.AddAsync(table);
        return table;
    }

    public async Task<IQueryable<Table>> GetAllAsync()
    {
        return await Task.FromResult(_context.Tables);
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

    public async Task SetAsBusyAsync(Guid id)
    {
        var table = await _context.Tables.FirstAsync(table => table.Id == id);
        table.SetBusy();

        await UpdateAsync(table);
    }


    public Task UpdateAsync(Table table)
    {
        _context.Tables.Update(table);
        return Task.CompletedTask;
    }
}
