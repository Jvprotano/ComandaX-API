using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using ComandaX.Application.Handlers.Subscriptions.Commands.InitiatePayment;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.Tests.Subscriptions.Commands.InitiatePayment;

public class InitiatePaymentCommandHandlerTests
{
    private readonly Mock<IAbacatePayService> _abacatePayServiceMock;
    private readonly Mock<ILogger<InitiatePaymentCommandHandler>> _loggerMock;
    private readonly InitiatePaymentCommandHandler _handler;

    public InitiatePaymentCommandHandlerTests()
    {
        _abacatePayServiceMock = new Mock<IAbacatePayService>();
        _loggerMock = new Mock<ILogger<InitiatePaymentCommandHandler>>();
        _handler = new InitiatePaymentCommandHandler(_abacatePayServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ReturnsSuccessfulPaymentResult()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var command = new InitiatePaymentCommand(
            TenantId: tenantId,
            CustomerEmail: "customer@example.com",
            CustomerName: "John Doe");

        var billingResult = new AbacatePayBillingResult(
            Success: true,
            BillingId: "billing-123",
            PaymentUrl: "https://abacate.pay/payment/123",
            PixQrCode: "00020126580014br.gov.bcb.pix...",
            PixCopyPaste: "00020126580014br.gov.bcb.pix...",
            CustomerId: "customer-123",
            ErrorMessage: null);

        _abacatePayServiceMock
            .Setup(s => s.CreateBillingAsync(
                tenantId,
                "customer@example.com",
                "John Doe",
                Subscription.MONTHLY_PRICE_CENTAVOS))
            .ReturnsAsync(billingResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("https://abacate.pay/payment/123", result.PaymentUrl);
        Assert.NotNull(result.PixQrCode);
        Assert.NotNull(result.PixCopyPaste);
        Assert.Null(result.ErrorMessage);
        _abacatePayServiceMock.Verify(
            s => s.CreateBillingAsync(
                tenantId,
                "customer@example.com",
                "John Doe",
                Subscription.MONTHLY_PRICE_CENTAVOS),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenAbacatePayFails_ReturnsFailureResult()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var command = new InitiatePaymentCommand(
            TenantId: tenantId,
            CustomerEmail: "customer@example.com",
            CustomerName: "John Doe");

        var billingResult = new AbacatePayBillingResult(
            Success: false,
            BillingId: null,
            PaymentUrl: null,
            PixQrCode: null,
            PixCopyPaste: null,
            CustomerId: null,
            ErrorMessage: "AbacatePay API error");

        _abacatePayServiceMock
            .Setup(s => s.CreateBillingAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(billingResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.PaymentUrl);
        Assert.Null(result.PixQrCode);
        Assert.Null(result.PixCopyPaste);
        Assert.Equal("AbacatePay API error", result.ErrorMessage);
    }

    [Fact]
    public async Task Handle_WhenAbacatePayReturnsNullError_ReturnsDefaultErrorMessage()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var command = new InitiatePaymentCommand(
            TenantId: tenantId,
            CustomerEmail: "customer@example.com",
            CustomerName: "John Doe");

        var billingResult = new AbacatePayBillingResult(
            Success: false,
            BillingId: null,
            PaymentUrl: null,
            PixQrCode: null,
            PixCopyPaste: null,
            CustomerId: null,
            ErrorMessage: null);

        _abacatePayServiceMock
            .Setup(s => s.CreateBillingAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(billingResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Failed to create payment", result.ErrorMessage);
    }

    [Fact]
    public async Task Handle_PassesCorrectPriceInCentavos()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var command = new InitiatePaymentCommand(
            TenantId: tenantId,
            CustomerEmail: "customer@example.com",
            CustomerName: "John Doe");

        var billingResult = new AbacatePayBillingResult(
            Success: true,
            BillingId: "billing-123",
            PaymentUrl: "https://abacate.pay/payment/123",
            PixQrCode: "qr-code",
            PixCopyPaste: "copy-paste",
            CustomerId: "customer-123",
            ErrorMessage: null);

        _abacatePayServiceMock
            .Setup(s => s.CreateBillingAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>()))
            .ReturnsAsync(billingResult);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _abacatePayServiceMock.Verify(
            s => s.CreateBillingAsync(
                tenantId,
                "customer@example.com",
                "John Doe",
                4990), // R$ 49.90 in centavos
            Times.Once);
    }
}
