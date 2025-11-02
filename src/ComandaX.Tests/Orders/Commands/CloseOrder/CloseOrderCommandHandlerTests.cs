using ComandaX.Application.Handlers.Orders.Commands.CloseOrder;
using ComandaX.Application.Interfaces;
using Moq;

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
}
