using ComandaX.Application.Exceptions;
using ComandaX.Application.Handlers.CustomerTabs.Commands.DeleteCustomerTab;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using Moq;
using Xunit;

namespace ComandaX.Tests.CustomerTabs.Commands.DeleteCustomerTab;

public class DeleteCustomerTabCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICustomerTabRepository> _customerTabRepositoryMock;
    private readonly DeleteCustomerTabCommandHandler _handler;

    public DeleteCustomerTabCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _customerTabRepositoryMock = new Mock<ICustomerTabRepository>();

        _unitOfWorkMock.Setup(u => u.CustomerTabs).Returns(_customerTabRepositoryMock.Object);

        _handler = new DeleteCustomerTabCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_SoftDeletesCustomerTab()
    {
        // Arrange
        var customerTabId = Guid.NewGuid();
        var command = new DeleteCustomerTabCommand(customerTabId);
        var customerTab = new CustomerTab("Test Tab", null);

        _customerTabRepositoryMock.Setup(r => r.GetByIdAsync(customerTabId))
            .ReturnsAsync(customerTab);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(customerTab.DeletedAt);
        _customerTabRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<CustomerTab>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CustomerTabNotFound_ThrowsRecordNotFoundException()
    {
        // Arrange
        var customerTabId = Guid.NewGuid();
        var command = new DeleteCustomerTabCommand(customerTabId);

        _customerTabRepositoryMock.Setup(r => r.GetByIdAsync(customerTabId))
            .ReturnsAsync((CustomerTab)null!);

        // Act & Assert
        await Assert.ThrowsAsync<RecordNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _customerTabRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<CustomerTab>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ValidCommand_SetsDeletedAtTimestamp()
    {
        // Arrange
        var customerTabId = Guid.NewGuid();
        var command = new DeleteCustomerTabCommand(customerTabId);
        var customerTab = new CustomerTab("Test Tab", null);
        var beforeDelete = DateTime.UtcNow;

        _customerTabRepositoryMock.Setup(r => r.GetByIdAsync(customerTabId))
            .ReturnsAsync(customerTab);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(customerTab.DeletedAt);
        Assert.True(customerTab.DeletedAt >= beforeDelete);
        Assert.True(customerTab.DeletedAt <= DateTime.UtcNow);
    }

    [Fact]
    public async Task Handle_ValidCommand_CallsRepositoryInCorrectOrder()
    {
        // Arrange
        var customerTabId = Guid.NewGuid();
        var command = new DeleteCustomerTabCommand(customerTabId);
        var customerTab = new CustomerTab("Test Tab", null);
        var callSequence = new List<string>();

        _customerTabRepositoryMock.Setup(r => r.GetByIdAsync(customerTabId))
            .ReturnsAsync(customerTab)
            .Callback(() => callSequence.Add("GetByIdAsync"));

        _customerTabRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<CustomerTab>()))
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

    [Fact]
    public async Task Handle_CustomerTabWithTable_OnlyDeletesCustomerTab()
    {
        // Arrange
        var customerTabId = Guid.NewGuid();
        var tableId = Guid.NewGuid();
        var command = new DeleteCustomerTabCommand(customerTabId);
        var customerTab = new CustomerTab("Test Tab", tableId);

        _customerTabRepositoryMock.Setup(r => r.GetByIdAsync(customerTabId))
            .ReturnsAsync(customerTab);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(customerTab.DeletedAt);
        Assert.Equal(tableId, customerTab.TableId); // Table reference should remain
        _customerTabRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<CustomerTab>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

