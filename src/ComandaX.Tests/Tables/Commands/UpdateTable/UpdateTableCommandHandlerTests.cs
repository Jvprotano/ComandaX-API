using ComandaX.Application.Exceptions;
using ComandaX.Application.Handlers.Tables.Commands.UpdateTable;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using Moq;
using Xunit;

namespace ComandaX.Tests.Tables.Commands.UpdateTable;

public class UpdateTableCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITableRepository> _tableRepositoryMock;
    private readonly UpdateTableHandler _handler;

    public UpdateTableCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _tableRepositoryMock = new Mock<ITableRepository>();

        _unitOfWorkMock.Setup(u => u.Tables).Returns(_tableRepositoryMock.Object);

        _handler = new UpdateTableHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_UpdatesTable()
    {
        // Arrange
        var tableId = Guid.NewGuid();
        var command = new UpdateTableCommand(tableId, 10);
        var table = new Table();
        table.SetNumber(5);

        _tableRepositoryMock.Setup(r => r.GetByIdAsync(tableId))
            .ReturnsAsync(table);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.Number);
        _tableRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Table>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_TableNotFound_ThrowsRecordNotFoundException()
    {
        // Arrange
        var tableId = Guid.NewGuid();
        var command = new UpdateTableCommand(tableId, 10);

        _tableRepositoryMock.Setup(r => r.GetByIdAsync(tableId))
            .ReturnsAsync((Table)null!);

        // Act & Assert
        await Assert.ThrowsAsync<RecordNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _tableRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Table>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}

