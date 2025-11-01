using Moq;
using Xunit;
using ComandaX.Application.Interfaces;
using ComandaX.Application.Orders.Commands.CreateOrder;
using ComandaX.Domain.Entities;

namespace ComandaX.Tests.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ITableRepository> _tableRepositoryMock;
    private readonly CreateOrderCommandHandler _handler;

    public CreateOrderCommandHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _tableRepositoryMock = new Mock<ITableRepository>();
        _handler = new CreateOrderCommandHandler(_orderRepositoryMock.Object, _productRepositoryMock.Object, _tableRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_CreateOrder_And_SetTableToBusy()
    {
        // Arrange
        var tableId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new CreateOrderCommand { TabId = tableId, ProductIds = new List<Guid> { productId } };
        var table = new Table(1);
        var product = new Product("Test Product", 10, 1);

        _tableRepositoryMock.Setup(r => r.GetByIdAsync(tableId)).ReturnsAsync(table);
        _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _orderRepositoryMock.Verify(r => r.CreateAsync(It.Is<Order>(o => o.TableId == tableId && o.OrderProducts.Count == 1)), Times.Once);
        _tableRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Table>(t => t.Id == table.Id && t.Status == ETableStatus.Busy)), Times.Once);
        Assert.NotEqual(Guid.Empty, result);
    }
}
