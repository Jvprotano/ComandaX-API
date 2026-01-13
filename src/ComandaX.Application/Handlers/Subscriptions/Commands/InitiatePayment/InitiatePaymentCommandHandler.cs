using Microsoft.Extensions.Logging;
using MediatR;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.Application.Handlers.Subscriptions.Commands.InitiatePayment;

/// <summary>
/// Handler for InitiatePaymentCommand.
/// Creates a PIX billing in AbacatePay and returns payment details.
/// </summary>
public class InitiatePaymentCommandHandler : IRequestHandler<InitiatePaymentCommand, InitiatePaymentResult>
{
    private readonly IAbacatePayService _abacatePayService;
    private readonly ILogger<InitiatePaymentCommandHandler> _logger;

    public InitiatePaymentCommandHandler(
        IAbacatePayService abacatePayService,
        ILogger<InitiatePaymentCommandHandler> logger)
    {
        _abacatePayService = abacatePayService;
        _logger = logger;
    }

    public async Task<InitiatePaymentResult> Handle(
        InitiatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Initiating payment for tenant {TenantId}", request.TenantId);

        // Create billing in AbacatePay
        var billingResult = await _abacatePayService.CreateBillingAsync(
            request.TenantId,
            request.CustomerEmail,
            request.CustomerName,
            Subscription.MONTHLY_PRICE_CENTAVOS);

        if (!billingResult.Success)
        {
            _logger.LogError("Failed to create billing for tenant {TenantId}: {Error}",
                request.TenantId, billingResult.ErrorMessage);

            return new InitiatePaymentResult(
                Success: false,
                PaymentUrl: null,
                PixQrCode: null,
                PixCopyPaste: null,
                ErrorMessage: billingResult.ErrorMessage ?? "Failed to create payment");
        }

        _logger.LogInformation("Payment initiated for tenant {TenantId}, billing {BillingId}",
            request.TenantId, billingResult.BillingId);

        return new InitiatePaymentResult(
            Success: true,
            PaymentUrl: billingResult.PaymentUrl,
            PixQrCode: billingResult.PixQrCode,
            PixCopyPaste: billingResult.PixCopyPaste,
            ErrorMessage: null);
    }
}
