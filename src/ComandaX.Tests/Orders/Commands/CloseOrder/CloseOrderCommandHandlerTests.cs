using ComandaX.Application.Handlers.Orders.Commands.CloseOrder;
using ComandaX.Application.Interfaces;
using Moq;

namespace ComandaX.Tests.Orders.Commands.CloseOrder;

public class CloseOrderCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<ITableRepository> _tableRepositoryMock;
    private readonly CloseOrderCommandHandler _handler;

    public CloseOrderCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _tableRepositoryMock = new Mock<ITableRepository>();

        // Setup Unit of Work to return mocked repositories
        _unitOfWorkMock.Setup(u => u.Orders).Returns(_orderRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.Tables).Returns(_tableRepositoryMock.Object);

        _handler = new CloseOrderCommandHandler(_unitOfWorkMock.Object);
    }
}
