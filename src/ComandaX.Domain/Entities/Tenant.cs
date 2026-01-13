namespace ComandaX.Domain.Entities;

/// <summary>
/// Represents a tenant (customer) in the multi-tenant system.
/// Each tenant has their own isolated data.
/// </summary>
public class Tenant : BaseEntity
{
    public Tenant()
    {
        Name = string.Empty;
    }

    public Tenant(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name.Trim();
        Description = description?.Trim();
    }

    /// <summary>
    /// The name of the tenant/customer.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Optional description of the tenant.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Whether the tenant is active. Inactive tenants cannot access the system.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Navigation property to the tenant's subscription.
    /// </summary>
    public Subscription? Subscription { get; set; }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name.Trim();
        EntityUpdated();
    }

    public void UpdateDescription(string? description)
    {
        Description = description?.Trim();
        EntityUpdated();
    }

    public void Activate()
    {
        IsActive = true;
        EntityUpdated();
    }

    public void Deactivate()
    {
        IsActive = false;
        EntityUpdated();
    }
}

