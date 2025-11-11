using ComandaX.Application.Handlers.Tables.Commands.CreateTable;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using Moq;
using Xunit;

namespace ComandaX.Tests.Tables.Commands.CreateTable;

public class CreateTableCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITableRepository> _tableRepositoryMock;
    private readonly CreateTableCommandHandler _handler;

    public CreateTableCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _tableRepositoryMock = new Mock<ITableRepository>();

        _unitOfWorkMock.Setup(u => u.Tables).Returns(_tableRepositoryMock.Object);

        _handler = new CreateTableCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommandWithNumber_CreatesTable()
    {
        // Arrange
        var command = new CreateTableCommand(5);

        _tableRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Table>()))
            .ReturnsAsync((Table table) => table);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Number);
        _tableRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Table>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommandWithoutNumber_CreatesTableWithDefaultNumber()
    {
        // Arrange
        var command = new CreateTableCommand(null);

        _tableRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Table>()))
            .ReturnsAsync((Table table) => table);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.Number);
        _tableRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Table>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

