using ComandaX.Domain.Enums;

namespace ComandaX.Domain.Entities;

/// <summary>
/// Represents a tenant's subscription to the service.
/// Subscriptions are tied to tenants, not individual users.
/// </summary>
public class Subscription : BaseEntity
{
    /// <summary>
    /// Price in centavos (Brazilian cents). R$49.90 = 4990 centavos.
    /// </summary>
    public const int MONTHLY_PRICE_CENTAVOS = 4990;

    /// <summary>
    /// Number of free trial days.
    /// </summary>
    public const int TRIAL_DAYS = 30;

    public Subscription()
    {
    }

    /// <summary>
    /// Creates a new trial subscription for a tenant.
    /// </summary>
    public Subscription(Guid tenantId)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID cannot be empty", nameof(tenantId));

        TenantId = tenantId;
        Status = SubscriptionStatusEnum.Trial;
        StartDate = DateTime.UtcNow;
        EndDate = DateTime.UtcNow.AddDays(TRIAL_DAYS);
    }

    /// <summary>
    /// Creates a long-lived, free subscription for a tenant.
    /// This is used while the product is free for all users.
    /// </summary>
    public static Subscription CreateFreeForOneYear(Guid tenantId)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID cannot be empty", nameof(tenantId));

        var subscription = new Subscription
        {
            TenantId = tenantId,
            Status = SubscriptionStatusEnum.Active,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddYears(1),
            PriceInCentavos = 0
        };

        return subscription;
    }

    /// <summary>
    /// The tenant this subscription belongs to.
    /// </summary>
    public Guid TenantId { get; private set; }

    /// <summary>
    /// Navigation property to the tenant.
    /// </summary>
    public Tenant? Tenant { get; set; }

    /// <summary>
    /// Current status of the subscription.
    /// </summary>
    public SubscriptionStatusEnum Status { get; private set; } = SubscriptionStatusEnum.Trial;

    /// <summary>
    /// When the current subscription period started.
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// When the current subscription period ends.
    /// </summary>
    public DateTime EndDate { get; private set; }

    /// <summary>
    /// The AbacatePay billing ID for the last payment.
    /// </summary>
    public string? AbacatePayBillingId { get; private set; }

    /// <summary>
    /// The AbacatePay customer ID.
    /// </summary>
    public string? AbacatePayCustomerId { get; private set; }

    /// <summary>
    /// Price paid in centavos for the current period.
    /// </summary>
    public int? PriceInCentavos { get; private set; }

    /// <summary>
    /// Checks if the subscription is currently valid (not expired).
    /// </summary>
    public bool IsValid => (Status == SubscriptionStatusEnum.Trial || Status == SubscriptionStatusEnum.Active)
                           && EndDate > DateTime.UtcNow;

    /// <summary>
    /// Checks if the subscription allows write operations.
    /// Only valid subscriptions can perform write operations.
    /// </summary>
    public bool AllowsWriteOperations => IsValid;

    /// <summary>
    /// Activates the subscription after a successful payment.
    /// </summary>
    public void Activate(string billingId, string? customerId, int priceInCentavos)
    {
        if (string.IsNullOrWhiteSpace(billingId))
            throw new ArgumentException("Billing ID cannot be empty", nameof(billingId));

        AbacatePayBillingId = billingId;
        AbacatePayCustomerId = customerId;
        PriceInCentavos = priceInCentavos;
        Status = SubscriptionStatusEnum.Active;

        // If currently in trial or expired, start fresh from now
        // If already active, extend from current end date
        if (Status == SubscriptionStatusEnum.Active && EndDate > DateTime.UtcNow)
        {
            EndDate = EndDate.AddDays(30);
        }
        else
        {
            StartDate = DateTime.UtcNow;
            EndDate = DateTime.UtcNow.AddDays(30);
        }

        EntityUpdated();
    }

    /// <summary>
    /// Marks the subscription as expired.
    /// </summary>
    public void Expire()
    {
        Status = SubscriptionStatusEnum.Expired;
        EntityUpdated();
    }

    /// <summary>
    /// Cancels the subscription.
    /// </summary>
    public void Cancel()
    {
        Status = SubscriptionStatusEnum.Cancelled;
        EntityUpdated();
    }

    /// <summary>
    /// Checks if the subscription is expiring soon (within 7 days).
    /// </summary>
    public bool IsExpiringSoon(int daysThreshold = 7)
    {
        return IsValid && EndDate <= DateTime.UtcNow.AddDays(daysThreshold);
    }
}

