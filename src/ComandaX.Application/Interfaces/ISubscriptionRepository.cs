using ComandaX.Domain.Entities;

namespace ComandaX.Application.Interfaces;

/// <summary>
/// Repository interface for Subscription entity operations.
/// </summary>
public interface ISubscriptionRepository
{
    /// <summary>
    /// Gets a subscription by its ID.
    /// </summary>
    Task<Subscription?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets the subscription for a specific tenant.
    /// </summary>
    Task<Subscription?> GetByTenantIdAsync(Guid tenantId);

    /// <summary>
    /// Gets all subscriptions that are expiring within the specified number of days.
    /// </summary>
    Task<IEnumerable<Subscription>> GetExpiringSoonAsync(int daysThreshold = 7);

    /// <summary>
    /// Gets all subscriptions that have expired (end date passed but status not yet updated).
    /// </summary>
    Task<IEnumerable<Subscription>> GetExpiredSubscriptionsAsync();

    /// <summary>
    /// Adds a new subscription.
    /// </summary>
    Task AddAsync(Subscription subscription);

    /// <summary>
    /// Updates an existing subscription.
    /// </summary>
    void Update(Subscription subscription);
}

