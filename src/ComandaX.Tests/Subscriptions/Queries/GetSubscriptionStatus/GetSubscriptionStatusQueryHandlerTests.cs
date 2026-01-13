using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using ComandaX.Application.Handlers.Subscriptions.Queries.GetSubscriptionStatus;
using ComandaX.Application.Interfaces;
using ComandaX.Application.Exceptions;
using ComandaX.Domain.Entities;

namespace ComandaX.Tests.Subscriptions.Queries.GetSubscriptionStatus;

public class GetSubscriptionStatusQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMock;
    private readonly Mock<ILogger<GetSubscriptionStatusQueryHandler>> _loggerMock;
    private readonly GetSubscriptionStatusQueryHandler _handler;

    public GetSubscriptionStatusQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
        _loggerMock = new Mock<ILogger<GetSubscriptionStatusQueryHandler>>();

        _unitOfWorkMock.Setup(u => u.Subscriptions).Returns(_subscriptionRepositoryMock.Object);
        _handler = new GetSubscriptionStatusQueryHandler(_unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidTrialSubscription_ReturnsSubscriptionStatusDto()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var query = new GetSubscriptionStatusQuery(TenantId: tenantId);
        var subscription = new Subscription(tenantId);

        _subscriptionRepositoryMock
            .Setup(r => r.GetByTenantIdAsync(tenantId))
            .ReturnsAsync(subscription);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(subscription.Id, result.Id);
        Assert.Equal("Trial", result.Status);
        Assert.Equal(subscription.StartDate, result.StartDate);
        Assert.Equal(subscription.EndDate, result.EndDate);
        Assert.True(result.IsValid);
        Assert.True(result.AllowsWriteOperations);
    }

    [Fact]
    public async Task Handle_WithActiveSubscription_ReturnsCorrectStatus()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var query = new GetSubscriptionStatusQuery(TenantId: tenantId);
        var subscription = new Subscription(tenantId);
        subscription.Activate("billing-123", "customer-123", 4990);

        _subscriptionRepositoryMock
            .Setup(r => r.GetByTenantIdAsync(tenantId))
            .ReturnsAsync(subscription);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Active", result.Status);
        Assert.True(result.IsValid);
        Assert.True(result.AllowsWriteOperations);
    }

    [Fact]
    public async Task Handle_CalculatesDaysRemainingCorrectly()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var query = new GetSubscriptionStatusQuery(TenantId: tenantId);
        var subscription = new Subscription(tenantId);

        _subscriptionRepositoryMock
            .Setup(r => r.GetByTenantIdAsync(tenantId))
            .ReturnsAsync(subscription);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        // Trial subscription should have approximately 30 days remaining
        Assert.InRange(result.DaysRemaining, 29, 31);
    }

    [Fact]
    public async Task Handle_WhenSubscriptionExpiredStatusButEndDateFuture_ReturnsDaysRemaining()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var query = new GetSubscriptionStatusQuery(TenantId: tenantId);
        var subscription = new Subscription(tenantId);
        subscription.Expire(); // Only changes status, not EndDate

        _subscriptionRepositoryMock
            .Setup(r => r.GetByTenantIdAsync(tenantId))
            .ReturnsAsync(subscription);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal("Expired", result.Status);
        // Days remaining should still be ~30 since EndDate hasn't changed
        Assert.InRange(result.DaysRemaining, 29, 31);
    }

    [Fact]
    public async Task Handle_WhenSubscriptionNotFound_ThrowsRecordNotFoundException()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var query = new GetSubscriptionStatusQuery(TenantId: tenantId);

        _subscriptionRepositoryMock
            .Setup(r => r.GetByTenantIdAsync(tenantId))
            .ReturnsAsync((Subscription?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<RecordNotFoundException>(
            async () => await _handler.Handle(query, CancellationToken.None));

        Assert.Contains($"Subscription not found for tenant {tenantId}", exception.Message);
    }

    [Fact]
    public async Task Handle_CalculatesPriceCorrectlyForMonthlySubscription()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var query = new GetSubscriptionStatusQuery(TenantId: tenantId);
        var subscription = new Subscription(tenantId);
        subscription.Activate("billing-123", "customer-123", 4990);

        _subscriptionRepositoryMock
            .Setup(r => r.GetByTenantIdAsync(tenantId))
            .ReturnsAsync(subscription);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(49.90m, result.PriceInBrl);
    }

    [Fact]
    public async Task Handle_WhenPriceInCentavosIsNull_UsesDefaultPrice()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var query = new GetSubscriptionStatusQuery(TenantId: tenantId);
        var subscription = new Subscription(tenantId);

        _subscriptionRepositoryMock
            .Setup(r => r.GetByTenantIdAsync(tenantId))
            .ReturnsAsync(subscription);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(49.90m, result.PriceInBrl); // Default monthly price for trial
    }

    [Fact]
    public async Task Handle_ReturnsCorrectIsExpiringSoonFlag()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var query = new GetSubscriptionStatusQuery(TenantId: tenantId);
        var subscription = new Subscription(tenantId);
        // Modify EndDate to be within 7 days using reflection
        var endDate = DateTime.UtcNow.AddDays(5);
        typeof(Subscription).GetProperty("EndDate")?.SetValue(subscription, endDate);

        _subscriptionRepositoryMock
            .Setup(r => r.GetByTenantIdAsync(tenantId))
            .ReturnsAsync(subscription);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsExpiringSoon);
    }
}
