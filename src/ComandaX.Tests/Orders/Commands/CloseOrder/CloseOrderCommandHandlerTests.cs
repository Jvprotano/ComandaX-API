using ComandaX.Application.Interfaces;
using ComandaX.Application.Orders.Commands.CloseOrder;
using ComandaX.Domain.Entities;
using Moq;
using Xunit;

namespace ComandaX.Tests.Orders.Commands.CloseOrder;

public class CloseOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<ITableRepository> _tableRepositoryMock;
    private readonly CloseOrderCommandHandler _handler;

    public CloseOrderCommandHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _tableRepositoryMock = new Mock<ITableRepository>();
        _handler = new CloseOrderCommandHandler(_orderRepositoryMock.Object, _tableRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_CloseOrder_And_SetTableToFree()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var tableId = Guid.NewGuid();
        var command = new CloseOrderCommand(orderId);
        var order = new Order();
        var table = new Table(1);

        _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);
        _tableRepositoryMock.Setup(r => r.GetByIdAsync(tableId)).ReturnsAsync(table);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _orderRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Order>(o => o.Id == orderId && o.Status == OrderStatusEnum.Closed)), Times.Once);
        _tableRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Table>(t => t.Id == table.Id && t.Status == ETableStatus.Free)), Times.Once);
    }
}
