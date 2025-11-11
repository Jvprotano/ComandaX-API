using ComandaX.Domain.Entities;
using ComandaX.Domain.Enums;
using Xunit;

namespace ComandaX.Tests.Domain.Entities;

public class OrderTests
{
    [Fact]
    public void Constructor_WithoutParameters_CreatesOrderWithDefaultValues()
    {
        // Arrange & Act
        var order = new Order();

        // Assert
        Assert.Equal(OrderStatusEnum.Closed, order.Status);
        Assert.Null(order.CustomerTabId);
        Assert.Empty(order.OrderProducts);
    }

    [Fact]
    public void Constructor_WithCustomerTabId_CreatesOrderWithCustomerTab()
    {
        // Arrange
        var customerTabId = Guid.NewGuid();

        // Act
        var order = new Order(customerTabId);

        // Assert
        Assert.Equal(OrderStatusEnum.Created, order.Status);
        Assert.Equal(customerTabId, order.CustomerTabId);
        Assert.Empty(order.OrderProducts);
    }

    [Fact]
    public void CloseOrder_WhenCalled_SetsStatusToClosed()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        order.StartPreparation();
        Assert.Equal(OrderStatusEnum.InPreparation, order.Status);

        // Act
        order.CloseOrder();

        // Assert
        Assert.Equal(OrderStatusEnum.Closed, order.Status);
    }

    [Fact]
    public void StartPreparation_WhenCalled_SetsStatusToInPreparation()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());

        // Act
        order.StartPreparation();

        // Assert
        Assert.Equal(OrderStatusEnum.InPreparation, order.Status);
    }

    [Fact]
    public void AddProduct_WithValidParameters_AddsProductToOrder()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var productId = Guid.NewGuid();

        // Act
        order.AddProduct(productId, 2, 10.99m);

        // Assert
        Assert.Single(order.OrderProducts);
        var orderProduct = order.OrderProducts.First();
        Assert.Equal(productId, orderProduct.ProductId);
        Assert.Equal(2, orderProduct.Quantity);
        Assert.Equal(21.98m, orderProduct.TotalPrice);
    }

    [Fact]
    public void AddProduct_WithZeroQuantity_ThrowsArgumentException()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var productId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => order.AddProduct(productId, 0, 10.99m));
    }

    [Fact]
    public void AddProduct_WithNegativeQuantity_ThrowsArgumentException()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var productId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => order.AddProduct(productId, -1, 10.99m));
    }

    [Fact]
    public void AddProduct_WithZeroPrice_ThrowsArgumentException()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var productId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => order.AddProduct(productId, 1, 0m));
    }

    [Fact]
    public void AddProduct_WithNegativePrice_ThrowsArgumentException()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var productId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => order.AddProduct(productId, 1, -5m));
    }

    [Fact]
    public void AddProduct_WithEmptyProductId_ThrowsArgumentException()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());

        // Act & Assert
        Assert.Throws<ArgumentException>(() => order.AddProduct(Guid.Empty, 1, 10.99m));
    }

    [Fact]
    public void AddProduct_MultipleProducts_AddsAllProducts()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var productId1 = Guid.NewGuid();
        var productId2 = Guid.NewGuid();

        // Act
        order.AddProduct(productId1, 2, 10.99m);
        order.AddProduct(productId2, 1, 5.50m);

        // Assert
        Assert.Equal(2, order.OrderProducts.Count);
    }
}

