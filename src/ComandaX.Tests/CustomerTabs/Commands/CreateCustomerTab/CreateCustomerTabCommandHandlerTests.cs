using ComandaX.Application.Exceptions;
using ComandaX.Application.Handlers.CustomerTabs.Commands.CreateCustomerTab;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using ComandaX.Domain.Enums;
using Moq;
using Xunit;

namespace ComandaX.Tests.CustomerTabs.Commands.CreateCustomerTab;

public class CreateCustomerTabCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICustomerTabRepository> _customerTabRepositoryMock;
    private readonly Mock<ITableRepository> _tableRepositoryMock;
    private readonly CreateCustomerTabCommandHandler _handler;

    public CreateCustomerTabCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _customerTabRepositoryMock = new Mock<ICustomerTabRepository>();
        _tableRepositoryMock = new Mock<ITableRepository>();

        _unitOfWorkMock.Setup(u => u.CustomerTabs).Returns(_customerTabRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.Tables).Returns(_tableRepositoryMock.Object);

        _handler = new CreateCustomerTabCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommandWithoutTable_CreatesCustomerTab()
    {
        // Arrange
        var command = new CreateCustomerTabCommand("Test Tab", null);

        _customerTabRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<CustomerTab>()))
            .ReturnsAsync((CustomerTab tab) => tab);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Tab", result.Name);
        Assert.Equal(CustomerTabStatusEnum.Open, result.Status);
        Assert.Null(result.TableId);

        _customerTabRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<CustomerTab>()), Times.Once);
        _tableRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommandWithTable_CreatesCustomerTabAndSetsTableBusy()
    {
        // Arrange
        var tableId = Guid.NewGuid();
        var command = new CreateCustomerTabCommand("Test Tab", tableId);
        var table = new Table();
        table.SetNumber(1);

        _customerTabRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<CustomerTab>()))
            .ReturnsAsync((CustomerTab tab) => tab);
        _tableRepositoryMock.Setup(r => r.GetByIdAsync(tableId))
            .ReturnsAsync(table);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Tab", result.Name);
        Assert.Equal(CustomerTabStatusEnum.Open, result.Status);
        Assert.Equal(tableId, result.TableId);
        Assert.Equal(TableStatusEnum.Busy, table.Status);

        _customerTabRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<CustomerTab>()), Times.Once);
        _tableRepositoryMock.Verify(r => r.GetByIdAsync(tableId), Times.Once);
        _tableRepositoryMock.Verify(r => r.UpdateAsync(table), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_TableNotFound_ThrowsRecordNotFoundException()
    {
        // Arrange
        var tableId = Guid.NewGuid();
        var command = new CreateCustomerTabCommand("Test Tab", tableId);

        _tableRepositoryMock.Setup(r => r.GetByIdAsync(tableId))
            .ReturnsAsync((Table?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<RecordNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains(tableId.ToString(), exception.Message);
        _customerTabRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<CustomerTab>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}

