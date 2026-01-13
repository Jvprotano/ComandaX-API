namespace ComandaX.Domain.Enums;

/// <summary>
/// Represents the status of a subscription.
/// </summary>
public enum SubscriptionStatusEnum
{
    /// <summary>
    /// User is in the free trial period (30 days).
    /// </summary>
    Trial = 0,

    /// <summary>
    /// Subscription is active and paid.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Subscription has expired (trial ended or payment not renewed).
    /// User has read-only access.
    /// </summary>
    Expired = 2,

    /// <summary>
    /// Subscription was cancelled by the user.
    /// </summary>
    Cancelled = 3
}

