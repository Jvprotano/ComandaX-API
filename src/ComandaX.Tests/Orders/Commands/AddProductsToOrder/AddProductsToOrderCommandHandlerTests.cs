using ComandaX.Application.Handlers.Orders.Commands.AddProductsToOrder;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using Moq;
using Xunit;

namespace ComandaX.Tests.Orders.Commands.AddProductsToOrder;

public class AddProductsToOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly AddProductsToOrderCommandHandler _handler;

    public AddProductsToOrderCommandHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _handler = new AddProductsToOrderCommandHandler(_orderRepositoryMock.Object, _productRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_AddProductsToOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var tabId = Guid.NewGuid();
        var command = new AddProductsToOrderCommand { OrderId = orderId, ProductIds = new List<Guid> { productId } };
        var order = new Order(tabId) { Id = orderId };
        var product = new Product("Test Product", 10);

        _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);
        _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _orderRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Order>(o => o.Id == orderId && o.OrderProducts.Count == 1)), Times.Once);
    }
}
