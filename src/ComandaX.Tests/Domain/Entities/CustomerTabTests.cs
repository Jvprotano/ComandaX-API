using ComandaX.Domain.Entities;
using ComandaX.Domain.Enums;
using Xunit;

namespace ComandaX.Tests.Domain.Entities;

public class CustomerTabTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesCustomerTab()
    {
        // Arrange & Act
        var customerTab = new CustomerTab("Test Tab", null);

        // Assert
        Assert.Equal("Test Tab", customerTab.Name);
        Assert.Equal(CustomerTabStatusEnum.Open, customerTab.Status);
        Assert.Null(customerTab.TableId);
    }

    [Fact]
    public void Constructor_WithTableId_CreatesCustomerTabWithTable()
    {
        // Arrange
        var tableId = Guid.NewGuid();

        // Act
        var customerTab = new CustomerTab("Test Tab", tableId);

        // Assert
        Assert.Equal("Test Tab", customerTab.Name);
        Assert.Equal(CustomerTabStatusEnum.Open, customerTab.Status);
        Assert.Equal(tableId, customerTab.TableId);
    }

    [Fact]
    public void Close_WhenCalled_SetsStatusToClosed()
    {
        // Arrange
        var customerTab = new CustomerTab("Test Tab", null);
        Assert.Equal(CustomerTabStatusEnum.Open, customerTab.Status);

        // Act
        customerTab.Close();

        // Assert
        Assert.Equal(CustomerTabStatusEnum.Closed, customerTab.Status);
    }

    [Fact]
    public void Close_WhenCalled_UpdatesUpdatedAtTimestamp()
    {
        // Arrange
        var customerTab = new CustomerTab("Test Tab", null);
        var originalUpdatedAt = customerTab.UpdatedAt;
        
        // Wait a bit to ensure timestamp changes
        Thread.Sleep(10);

        // Act
        customerTab.Close();

        // Assert
        Assert.True(customerTab.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void DefaultConstructor_CreatesCustomerTabWithDefaultValues()
    {
        // Arrange & Act
        var customerTab = new CustomerTab();

        // Assert
        Assert.NotNull(customerTab.Name);
        Assert.Equal(string.Empty, customerTab.Name);
        Assert.Equal(CustomerTabStatusEnum.Open, customerTab.Status);
    }
}

