using ComandaX.Application.Exceptions;
using ComandaX.Application.Handlers.CustomerTabs.Commands.CloseCustomerTab;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using ComandaX.Domain.Enums;
using Moq;
using Xunit;

namespace ComandaX.Tests.CustomerTabs.Commands.CloseCustomerTab;

public class CloseCustomerTabCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICustomerTabRepository> _customerTabRepositoryMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<ITableRepository> _tableRepositoryMock;
    private readonly CloseCustomerTabCommandHandler _handler;

    public CloseCustomerTabCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _customerTabRepositoryMock = new Mock<ICustomerTabRepository>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _tableRepositoryMock = new Mock<ITableRepository>();

        // Setup Unit of Work to return mocked repositories
        _unitOfWorkMock.Setup(u => u.CustomerTabs).Returns(_customerTabRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.Orders).Returns(_orderRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.Tables).Returns(_tableRepositoryMock.Object);

        _handler = new CloseCustomerTabCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCustomerTabWithoutTable_ClosesTabAndOrders()
    {
        // Arrange
        var customerTabId = Guid.NewGuid();
        var command = new CloseCustomerTabCommand(customerTabId);
        var customerTab = new CustomerTab("Test Tab", null);

        var order1 = new Order(customerTabId);
        var order2 = new Order(customerTabId);
        var orders = new List<Order> { order1, order2 };

        _customerTabRepositoryMock.Setup(r => r.GetByIdAsync(customerTabId))
            .ReturnsAsync(customerTab);
        _orderRepositoryMock.Setup(r => r.GetByCustomerTabIdAsync(customerTabId))
            .ReturnsAsync(orders);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(CustomerTabStatusEnum.Closed, customerTab.Status);
        Assert.All(orders, order => Assert.Equal(OrderStatusEnum.Closed, order.Status));

        _customerTabRepositoryMock.Verify(r => r.UpdateAsync(customerTab), Times.Once);
        _orderRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Exactly(2));
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _tableRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ValidCustomerTabWithTable_ClosesTabOrdersAndFreesTable()
    {
        // Arrange
        var customerTabId = Guid.NewGuid();
        var tableId = Guid.NewGuid();
        var command = new CloseCustomerTabCommand(customerTabId);
        var customerTab = new CustomerTab("Test Tab", tableId);
        var table = new Table();
        table.SetNumber(1);
        table.SetBusy();

        var order1 = new Order(customerTabId);
        var order2 = new Order(customerTabId);
        var orders = new List<Order> { order1, order2 };

        _customerTabRepositoryMock.Setup(r => r.GetByIdAsync(customerTabId))
            .ReturnsAsync(customerTab);
        _orderRepositoryMock.Setup(r => r.GetByCustomerTabIdAsync(customerTabId))
            .ReturnsAsync(orders);
        _tableRepositoryMock.Setup(r => r.GetByIdAsync(tableId))
            .ReturnsAsync(table);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(CustomerTabStatusEnum.Closed, customerTab.Status);
        Assert.All(orders, order => Assert.Equal(OrderStatusEnum.Closed, order.Status));
        Assert.Equal(TableStatusEnum.Free, table.Status);

        _customerTabRepositoryMock.Verify(r => r.UpdateAsync(customerTab), Times.Once);
        _orderRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Exactly(2));
        _tableRepositoryMock.Verify(r => r.GetByIdAsync(tableId), Times.Once);
        _tableRepositoryMock.Verify(r => r.UpdateAsync(table), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CustomerTabWithNoOrders_ClosesTabOnly()
    {
        // Arrange
        var customerTabId = Guid.NewGuid();
        var command = new CloseCustomerTabCommand(customerTabId);
        var customerTab = new CustomerTab("Test Tab", null);
        var orders = new List<Order>();

        _customerTabRepositoryMock.Setup(r => r.GetByIdAsync(customerTabId))
            .ReturnsAsync(customerTab);
        _orderRepositoryMock.Setup(r => r.GetByCustomerTabIdAsync(customerTabId))
            .ReturnsAsync(orders);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(CustomerTabStatusEnum.Closed, customerTab.Status);

        _customerTabRepositoryMock.Verify(r => r.UpdateAsync(customerTab), Times.Once);
        _orderRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CustomerTabWithAlreadyClosedOrders_OnlyClosesOpenOrders()
    {
        // Arrange
        var customerTabId = Guid.NewGuid();
        var command = new CloseCustomerTabCommand(customerTabId);
        var customerTab = new CustomerTab("Test Tab", null);

        var order1 = new Order(customerTabId);
        var order2 = new Order(customerTabId);
        order2.CloseOrder(); // Already closed
        var orders = new List<Order> { order1, order2 };

        _customerTabRepositoryMock.Setup(r => r.GetByIdAsync(customerTabId))
            .ReturnsAsync(customerTab);
        _orderRepositoryMock.Setup(r => r.GetByCustomerTabIdAsync(customerTabId))
            .ReturnsAsync(orders);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(CustomerTabStatusEnum.Closed, customerTab.Status);
        Assert.Equal(OrderStatusEnum.Closed, order1.Status);
        Assert.Equal(OrderStatusEnum.Closed, order2.Status);

        _customerTabRepositoryMock.Verify(r => r.UpdateAsync(customerTab), Times.Once);
        _orderRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Once); // Only one order updated
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CustomerTabNotFound_ThrowsRecordNotFoundException()
    {
        // Arrange
        var customerTabId = Guid.NewGuid();
        var command = new CloseCustomerTabCommand(customerTabId);

        _customerTabRepositoryMock.Setup(r => r.GetByIdAsync(customerTabId))
            .ReturnsAsync((CustomerTab?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<RecordNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains(customerTabId.ToString(), exception.Message);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_TableNotFound_ThrowsRecordNotFoundException()
    {
        // Arrange
        var customerTabId = Guid.NewGuid();
        var tableId = Guid.NewGuid();
        var command = new CloseCustomerTabCommand(customerTabId);
        var customerTab = new CustomerTab("Test Tab", tableId);
        var orders = new List<Order>();

        _customerTabRepositoryMock.Setup(r => r.GetByIdAsync(customerTabId))
            .ReturnsAsync(customerTab);
        _orderRepositoryMock.Setup(r => r.GetByCustomerTabIdAsync(customerTabId))
            .ReturnsAsync(orders);
        _tableRepositoryMock.Setup(r => r.GetByIdAsync(tableId))
            .ReturnsAsync((Table?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<RecordNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains(tableId.ToString(), exception.Message);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_MultipleOrdersWithDifferentStatuses_ClosesOnlyNonClosedOrders()
    {
        // Arrange
        var customerTabId = Guid.NewGuid();
        var command = new CloseCustomerTabCommand(customerTabId);
        var customerTab = new CustomerTab("Test Tab", null);

        var order1 = new Order(customerTabId); // Created
        var order2 = new Order(customerTabId);
        order2.StartPreparation(); // InPreparation
        var order3 = new Order(customerTabId);
        order3.CloseOrder(); // Already Closed
        var orders = new List<Order> { order1, order2, order3 };

        _customerTabRepositoryMock.Setup(r => r.GetByIdAsync(customerTabId))
            .ReturnsAsync(customerTab);
        _orderRepositoryMock.Setup(r => r.GetByCustomerTabIdAsync(customerTabId))
            .ReturnsAsync(orders);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(CustomerTabStatusEnum.Closed, customerTab.Status);
        Assert.Equal(OrderStatusEnum.Closed, order1.Status);
        Assert.Equal(OrderStatusEnum.Closed, order2.Status);
        Assert.Equal(OrderStatusEnum.Closed, order3.Status);

        _customerTabRepositoryMock.Verify(r => r.UpdateAsync(customerTab), Times.Once);
        _orderRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Exactly(2)); // Only 2 orders updated
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

