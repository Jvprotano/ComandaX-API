using ComandaX.Application.Handlers.CustomerTabs.Queries.GetCustomerTabs;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using ComandaX.Domain.Enums;
using Moq;
using Xunit;

namespace ComandaX.Tests.CustomerTabs.Queries.GetCustomerTabs;

public class GetCustomerTabsHandlerTests
{
    private readonly Mock<ICustomerTabRepository> _customerTabRepositoryMock;
    private readonly GetCustomerTabsHandler _handler;

    public GetCustomerTabsHandlerTests()
    {
        _customerTabRepositoryMock = new Mock<ICustomerTabRepository>();
        _handler = new GetCustomerTabsHandler(_customerTabRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_NoStatusFilter_ReturnsAllCustomerTabs()
    {
        // Arrange
        var query = new GetCustomerTabsQuery(null);
        var customerTab1 = new CustomerTab("Tab 1", null);
        var customerTab2 = new CustomerTab("Tab 2", null);
        customerTab2.Close();
        
        var customerTabs = new List<CustomerTab> { customerTab1, customerTab2 }.AsQueryable();

        _customerTabRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(customerTabs);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        _customerTabRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WithOpenStatusFilter_ReturnsOnlyOpenCustomerTabs()
    {
        // Arrange
        var query = new GetCustomerTabsQuery(CustomerTabStatusEnum.Open);
        var customerTab1 = new CustomerTab("Tab 1", null);
        var customerTab2 = new CustomerTab("Tab 2", null);
        customerTab2.Close();
        
        var customerTabs = new List<CustomerTab> { customerTab1, customerTab2 }.AsQueryable();

        _customerTabRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(customerTabs);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Single(resultList);
        Assert.Equal("Tab 1", resultList[0].Name);
        Assert.Equal(CustomerTabStatusEnum.Open, resultList[0].Status);
        _customerTabRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WithClosedStatusFilter_ReturnsOnlyClosedCustomerTabs()
    {
        // Arrange
        var query = new GetCustomerTabsQuery(CustomerTabStatusEnum.Closed);
        var customerTab1 = new CustomerTab("Tab 1", null);
        var customerTab2 = new CustomerTab("Tab 2", null);
        customerTab2.Close();
        
        var customerTabs = new List<CustomerTab> { customerTab1, customerTab2 }.AsQueryable();

        _customerTabRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(customerTabs);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Single(resultList);
        Assert.Equal("Tab 2", resultList[0].Name);
        Assert.Equal(CustomerTabStatusEnum.Closed, resultList[0].Status);
        _customerTabRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_EmptyRepository_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetCustomerTabsQuery(null);
        var customerTabs = new List<CustomerTab>().AsQueryable();

        _customerTabRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(customerTabs);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _customerTabRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }
}

