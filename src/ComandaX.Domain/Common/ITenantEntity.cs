namespace ComandaX.Domain.Common;

/// <summary>
/// Interface for entities that belong to a specific tenant.
/// Entities implementing this interface will have their data isolated per tenant.
/// </summary>
public interface ITenantEntity
{
    /// <summary>
    /// The ID of the tenant that owns this entity.
    /// </summary>
    Guid TenantId { get; }

    /// <summary>
    /// Sets the tenant ID for this entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID to set.</param>
    void SetTenantId(Guid tenantId);
}

