using ComandaX.Domain.Common;

namespace ComandaX.Domain.Entities;

public class User : BaseEntity, ITenantEntity
{
    public User()
    {
        Email = string.Empty;
        Role = string.Empty;
    }

    public User(string email, string role, Guid tenantId)
    {
        Email = email;
        Role = role;
        TenantId = tenantId;
    }

    public Guid TenantId { get; private set; }
    public string Email { get; private set; }
    public string Role { get; private set; }
    public Tenant? Tenant { get; set; }

    public void SetTenantId(Guid tenantId)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID cannot be empty", nameof(tenantId));
        TenantId = tenantId;
    }
}
