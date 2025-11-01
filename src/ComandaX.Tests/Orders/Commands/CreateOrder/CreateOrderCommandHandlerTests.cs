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
}
