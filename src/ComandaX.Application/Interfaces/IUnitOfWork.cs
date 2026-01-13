namespace ComandaX.Application.Interfaces;

/// <summary>
/// Unit of Work pattern interface for managing transactions and coordinating repository operations.
/// Ensures that multiple repository operations can be grouped into a single transaction.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the Product repository.
    /// </summary>
    IProductRepository Products { get; }

    /// <summary>
    /// Gets the Order repository.
    /// </summary>
    IOrderRepository Orders { get; }

    /// <summary>
    /// Gets the CustomerTab repository.
    /// </summary>
    ICustomerTabRepository CustomerTabs { get; }

    /// <summary>
    /// Gets the Table repository.
    /// </summary>
    ITableRepository Tables { get; }

    /// <summary>
    /// Gets the ProductCategory repository.
    /// </summary>
    IProductCategoryRepository ProductCategories { get; }

    /// <summary>
    /// Gets the User repository.
    /// </summary>
    IUserRepository Users { get; }

    /// <summary>
    /// Gets the Tenant repository.
    /// </summary>
    ITenantRepository Tenants { get; }

    /// <summary>
    /// Gets the Subscription repository.
    /// </summary>
    ISubscriptionRepository Subscriptions { get; }

    /// <summary>
    /// Saves all changes made in this unit of work to the database.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}

