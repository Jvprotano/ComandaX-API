using ComandaX.Application.Handlers.Orders.Commands.AddProductsToOrder;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using Moq;
using Xunit;

namespace ComandaX.Tests.Orders.Commands.AddProductsToOrder;

public class AddProductsToOrderCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly AddProductsToOrderCommandHandler _handler;

    public AddProductsToOrderCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();

        // Setup Unit of Work to return mocked repositories
        _unitOfWorkMock.Setup(u => u.Orders).Returns(_orderRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.Products).Returns(_productRepositoryMock.Object);

        _handler = new AddProductsToOrderCommandHandler(_unitOfWorkMock.Object);
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
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
