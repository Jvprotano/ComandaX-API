using ComandaX.Application.Interfaces;
using ComandaX.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace ComandaX.Infrastructure.Persistence;

/// <summary>
/// Implementation of the Unit of Work pattern.
/// Coordinates the work of multiple repositories and maintains a single database transaction.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    // Lazy initialization of repositories
    private IProductRepository? _products;
    private IOrderRepository? _orders;
    private ICustomerTabRepository? _customerTabs;
    private ITableRepository? _tables;
    private IProductCategoryRepository? _productCategories;
    private IUserRepository? _users;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets the Product repository. Lazy-loaded on first access.
    /// </summary>
    public IProductRepository Products
    {
        get
        {
            _products ??= new ProductRepository(_context);
            return _products;
        }
    }

    /// <summary>
    /// Gets the Order repository. Lazy-loaded on first access.
    /// </summary>
    public IOrderRepository Orders
    {
        get
        {
            _orders ??= new OrderRepository(_context);
            return _orders;
        }
    }

    /// <summary>
    /// Gets the CustomerTab repository. Lazy-loaded on first access.
    /// </summary>
    public ICustomerTabRepository CustomerTabs
    {
        get
        {
            _customerTabs ??= new CustomerTabRepository(_context);
            return _customerTabs;
        }
    }

    /// <summary>
    /// Gets the Table repository. Lazy-loaded on first access.
    /// </summary>
    public ITableRepository Tables
    {
        get
        {
            _tables ??= new TableRepository(_context);
            return _tables;
        }
    }

    /// <summary>
    /// Gets the ProductCategory repository. Lazy-loaded on first access.
    /// </summary>
    public IProductCategoryRepository ProductCategories
    {
        get
        {
            _productCategories ??= new ProductCategoryRepository(_context);
            return _productCategories;
        }
    }

    /// <summary>
    /// Gets the User repository. Lazy-loaded on first access.
    /// </summary>
    public IUserRepository Users
    {
        get
        {
            _users ??= new UserRepository(_context);
            return _users;
        }
    }

    /// <summary>
    /// Saves all changes made in this unit of work to the database.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <summary>
    /// Disposes the unit of work and releases all resources.
    /// </summary>
    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}

