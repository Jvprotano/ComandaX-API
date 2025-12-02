namespace ComandaX.Application.Interfaces;

/// <summary>
/// Service for accessing the current tenant context.
/// Provides the tenant ID for the current request.
/// </summary>
public interface ITenantService
{
    /// <summary>
    /// Gets the current tenant ID from the request context.
    /// </summary>
    /// <returns>The tenant ID, or null if no tenant is set.</returns>
    Guid? GetCurrentTenantId();

    /// <summary>
    /// Sets the current tenant ID for the request context.
    /// </summary>
    /// <param name="tenantId">The tenant ID to set.</param>
    void SetCurrentTenantId(Guid tenantId);
}

