using ComandaX.Domain.Entities;
using ComandaX.Domain.Enums;
using ComandaX.Tests.Helpers.Builders;
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
        const decimal PRODUCT_PRICE = 10.99m;
        const decimal QUANTITY = 2m;
        decimal expectedTotalPrice = PRODUCT_PRICE * QUANTITY;

        var order = new Order(Guid.NewGuid());
        var product = ProductBuilder.New().WithPrice(PRODUCT_PRICE).Build();

        // Act
        order.AddProduct(product, QUANTITY);

        // Assert
        Assert.Single(order.OrderProducts);
        var orderProduct = order.OrderProducts.First();
        Assert.Equal(product.Id, orderProduct.ProductId);
        Assert.Equal(QUANTITY, orderProduct.Quantity);
        Assert.Equal(expectedTotalPrice, orderProduct.TotalPrice);
    }

    [Fact]
    public void AddProduct_WithZeroQuantity_ThrowsArgumentException()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var product = ProductBuilder.New().Build();


        // Act & Assert
        Assert.Throws<ArgumentException>(() => order.AddProduct(product, 0));
    }

    [Fact]
    public void AddProduct_WithNegativeQuantity_ThrowsArgumentException()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var product = ProductBuilder.New().Build();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => order.AddProduct(product, -1));
    }

    [Fact]
    public void AddProduct_WithZeroPrice_ThrowsArgumentException()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var product = ProductBuilder.New().WithPrice(0m).Build();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => order.AddProduct(product, 1));
    }

    [Fact]
    public void AddProduct_WithNegativePrice_ThrowsArgumentException()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var product = ProductBuilder.New().WithPrice(-10m).Build();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => order.AddProduct(product, 1));
    }

    [Fact]
    public void AddProduct_WithEmptyProductId_ThrowsArgumentException()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var product = ProductBuilder.New().WithId(Guid.Empty).Build();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => order.AddProduct(product, 10.99m));
    }

    [Fact]
    public void AddProduct_MultipleProducts_AddsAllProducts()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());

        var products = ProductBuilder.New().BuildList(2);

        // Act
        order.AddProduct(products[0], 10.99m);
        order.AddProduct(products[1], 5.50m);

        // Assert
        Assert.Equal(2, order.OrderProducts.Count);
    }
}

