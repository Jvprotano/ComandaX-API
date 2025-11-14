using ComandaX.Application.Exceptions;
using ComandaX.Application.Handlers.Orders.Commands.DeleteOrder;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using Moq;
using Xunit;

namespace ComandaX.Tests.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly DeleteOrderCommandHandler _handler;

    public DeleteOrderCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _orderRepositoryMock = new Mock<IOrderRepository>();

        _unitOfWorkMock.Setup(u => u.Orders).Returns(_orderRepositoryMock.Object);

        _handler = new DeleteOrderCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_SoftDeletesOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new DeleteOrderCommand(orderId);
        var order = new Order();

        _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId))
            .ReturnsAsync(order);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(order.DeletedAt);
        _orderRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_OrderNotFound_ThrowsRecordNotFoundException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new DeleteOrderCommand(orderId);

        _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId))
            .ReturnsAsync((Order)null!);

        // Act & Assert
        await Assert.ThrowsAsync<RecordNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _orderRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ValidCommand_SetsDeletedAtTimestamp()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new DeleteOrderCommand(orderId);
        var order = new Order();
        var beforeDelete = DateTime.UtcNow;

        _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId))
            .ReturnsAsync(order);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(order.DeletedAt);
        Assert.True(order.DeletedAt >= beforeDelete);
        Assert.True(order.DeletedAt <= DateTime.UtcNow);
    }

    [Fact]
    public async Task Handle_ValidCommand_CallsRepositoryInCorrectOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new DeleteOrderCommand(orderId);
        var order = new Order();
        var callSequence = new List<string>();

        _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId))
            .ReturnsAsync(order)
            .Callback(() => callSequence.Add("GetByIdAsync"));

        _orderRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Order>()))
            .Callback(() => callSequence.Add("UpdateAsync"))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Callback(() => callSequence.Add("SaveChangesAsync"))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(new[] { "GetByIdAsync", "UpdateAsync", "SaveChangesAsync" }, callSequence);
    }
}

