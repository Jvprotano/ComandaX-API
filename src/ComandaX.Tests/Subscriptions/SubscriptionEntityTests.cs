using Xunit;
using ComandaX.Domain.Entities;
using ComandaX.Domain.Enums;

namespace ComandaX.Tests.Subscriptions;

public class SubscriptionEntityTests
{
    [Fact]
    public void Constructor_CreatesTrialSubscription()
    {
        // Arrange
        var tenantId = Guid.NewGuid();

        // Act
        var subscription = new Subscription(tenantId);

        // Assert
        Assert.Equal(tenantId, subscription.TenantId);
        Assert.Equal(SubscriptionStatusEnum.Trial, subscription.Status);
        Assert.True(subscription.IsValid);
        Assert.True(subscription.AllowsWriteOperations);
    }

    [Fact]
    public void Activate_SetsStatusToActive()
    {
        // Arrange
        var subscription = new Subscription(Guid.NewGuid());

        // Act
        subscription.Activate("billing-123", "customer-123", 4990);

        // Assert
        Assert.Equal(SubscriptionStatusEnum.Active, subscription.Status);
        Assert.Equal("billing-123", subscription.AbacatePayBillingId);
        Assert.Equal("customer-123", subscription.AbacatePayCustomerId);
        Assert.Equal(4990, subscription.PriceInCentavos);
    }

    [Fact]
    public void Activate_UpdatesEndDateToFutureDate()
    {
        // Arrange
        var subscription = new Subscription(Guid.NewGuid());
        var beforeActivation = DateTime.UtcNow;

        // Act
        subscription.Activate("billing-123", "customer-123", 4990);

        // Assert
        Assert.True(subscription.EndDate > beforeActivation.AddDays(20));
        Assert.True(subscription.EndDate < beforeActivation.AddMonths(2));
    }

    [Fact]
    public void Expire_SetsStatusToExpired()
    {
        // Arrange
        var subscription = new Subscription(Guid.NewGuid());
        subscription.Activate("billing-123", "customer-123", 4990);

        // Act
        subscription.Expire();

        // Assert
        Assert.Equal(SubscriptionStatusEnum.Expired, subscription.Status);
    }

    [Fact]
    public void Cancel_SetsStatusToCancelled()
    {
        // Arrange
        var subscription = new Subscription(Guid.NewGuid());

        // Act
        subscription.Cancel();

        // Assert
        Assert.Equal(SubscriptionStatusEnum.Cancelled, subscription.Status);
    }

    [Fact]
    public void IsValid_ReturnsTrueForNewTrialSubscription()
    {
        // Arrange
        var subscription = new Subscription(Guid.NewGuid());

        // Act
        var isValid = subscription.IsValid;

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void IsValid_ReturnsTrueForActiveSubscription()
    {
        // Arrange
        var subscription = new Subscription(Guid.NewGuid());
        subscription.Activate("billing-123", "customer-123", 4990);

        // Act
        var isValid = subscription.IsValid;

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void IsValid_ReturnsFalseForCancelledSubscription()
    {
        // Arrange
        var subscription = new Subscription(Guid.NewGuid());
        subscription.Cancel();

        // Act
        var isValid = subscription.IsValid;

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void IsExpiringSoon_ReturnsTrueWhenLessThanSevenDaysRemain()
    {
        // Arrange
        var subscription = new Subscription(Guid.NewGuid());
        // Modify EndDate by using reflection since it's private
        var endDate = DateTime.UtcNow.AddDays(5);
        typeof(Subscription).GetProperty("EndDate")?.SetValue(subscription, endDate);

        // Act
        var isExpiringSoon = subscription.IsExpiringSoon();

        // Assert
        Assert.True(isExpiringSoon);
    }

    [Fact]
    public void IsExpiringSoon_ReturnsFalseWhenMoreThanSevenDaysRemain()
    {
        // Arrange
        var subscription = new Subscription(Guid.NewGuid());
        // Default trial subscription has 30 days, so this should return false
        
        // Act
        var isExpiringSoon = subscription.IsExpiringSoon();

        // Assert
        Assert.False(isExpiringSoon);
    }

    [Fact]
    public void AllowsWriteOperations_ReturnsTrueForValidSubscriptions()
    {
        // Arrange
        var subscriptions = new[]
        {
            new Subscription(Guid.NewGuid()),
            new Subscription(Guid.NewGuid())
        };
        subscriptions[1].Activate("billing-123", "customer-123", 4990);

        // Act & Assert
        foreach (var subscription in subscriptions)
        {
            Assert.True(subscription.AllowsWriteOperations);
        }
    }

    [Fact]
    public void AllowsWriteOperations_ReturnsFalseForCancelledSubscriptions()
    {
        // Arrange
        var subscription = new Subscription(Guid.NewGuid());
        subscription.Cancel();

        // Act
        var allowsWriteOperations = subscription.AllowsWriteOperations;

        // Assert
        Assert.False(allowsWriteOperations);
    }

    [Fact]
    public void MONTHLY_PRICE_CENTAVOS_EqualsFortyNineNinetyInCentavos()
    {
        // Assert
        Assert.Equal(4990, Subscription.MONTHLY_PRICE_CENTAVOS);
    }

    [Fact]
    public void TRIAL_DAYS_EqualsThirty()
    {
        // Assert
        Assert.Equal(30, Subscription.TRIAL_DAYS);
    }

    [Fact]
    public void Constructor_ThrowsWhenTenantIdIsEmpty()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Subscription(Guid.Empty));
        Assert.Contains("Tenant ID cannot be empty", exception.Message);
    }
}
