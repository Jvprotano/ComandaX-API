using ComandaX.Application.Interfaces;

namespace ComandaX.Infrastructure.Services;

/// <summary>
/// Implementation of ITenantService that stores the current tenant ID
/// in an AsyncLocal for thread-safe access within the request scope.
/// </summary>
public class TenantService : ITenantService
{
    private static readonly AsyncLocal<Guid?> _currentTenantId = new();

    public Guid? GetCurrentTenantId()
    {
        return _currentTenantId.Value;
    }

    public void SetCurrentTenantId(Guid tenantId)
    {
        _currentTenantId.Value = tenantId;
    }
}

