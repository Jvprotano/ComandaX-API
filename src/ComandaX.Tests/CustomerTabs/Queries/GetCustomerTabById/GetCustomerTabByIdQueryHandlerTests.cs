using ComandaX.Application.Exceptions;
using ComandaX.Application.Handlers.CustomerTabs.Queries.GetCustomerTabById;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using Moq;
using Xunit;

namespace ComandaX.Tests.CustomerTabs.Queries.GetCustomerTabById;

public class GetCustomerTabByIdQueryHandlerTests
{
    private readonly Mock<ICustomerTabRepository> _customerTabRepositoryMock;
    private readonly GetCustomerTabByIdQueryHandler _handler;

    public GetCustomerTabByIdQueryHandlerTests()
    {
        _customerTabRepositoryMock = new Mock<ICustomerTabRepository>();
        _handler = new GetCustomerTabByIdQueryHandler(_customerTabRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidId_ReturnsCustomerTab()
    {
        // Arrange
        var customerTabId = Guid.NewGuid();
        var query = new GetCustomerTabByIdQuery(customerTabId);
        var customerTab = new CustomerTab("Test Tab", null);

        _customerTabRepositoryMock.Setup(r => r.GetByIdAsync(customerTabId))
            .ReturnsAsync(customerTab);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Tab", result.Name);
        _customerTabRepositoryMock.Verify(r => r.GetByIdAsync(customerTabId), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidId_ThrowsRecordNotFoundException()
    {
        // Arrange
        var customerTabId = Guid.NewGuid();
        var query = new GetCustomerTabByIdQuery(customerTabId);

        _customerTabRepositoryMock.Setup(r => r.GetByIdAsync(customerTabId))
            .ReturnsAsync((CustomerTab?)null);

        // Act & Assert
        await Assert.ThrowsAsync<RecordNotFoundException>(
            () => _handler.Handle(query, CancellationToken.None));
        
        _customerTabRepositoryMock.Verify(r => r.GetByIdAsync(customerTabId), Times.Once);
    }
}

