using ComandaX.Domain.Entities;

namespace ComandaX.Application.Interfaces;

/// <summary>
/// Service interface for sending subscription-related notifications.
/// </summary>
public interface ISubscriptionNotificationService
{
    /// <summary>
    /// Sends a welcome email when a new trial subscription is created.
    /// </summary>
    Task SendTrialStartedEmailAsync(Tenant tenant, Subscription subscription, string email);

    /// <summary>
    /// Sends a reminder email when the subscription is expiring soon.
    /// </summary>
    Task SendExpiringNotificationAsync(Tenant tenant, Subscription subscription, string email);

    /// <summary>
    /// Sends a notification when the subscription has expired.
    /// </summary>
    Task SendExpiredNotificationAsync(Tenant tenant, Subscription subscription, string email);

    /// <summary>
    /// Sends a confirmation email when a subscription is activated/renewed.
    /// </summary>
    Task SendSubscriptionActivatedEmailAsync(Tenant tenant, Subscription subscription, string email);
}

