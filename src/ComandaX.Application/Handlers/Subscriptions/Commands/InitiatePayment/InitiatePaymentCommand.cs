using MediatR;

namespace ComandaX.Application.Handlers.Subscriptions.Commands.InitiatePayment;

/// <summary>
/// Command to initiate a subscription payment via AbacatePay.
/// </summary>
public record InitiatePaymentCommand(
    Guid TenantId,
    string CustomerEmail,
    string CustomerName
) : IRequest<InitiatePaymentResult>;

/// <summary>
/// Result of initiating a payment.
/// </summary>
public record InitiatePaymentResult(
    bool Success,
    string? PaymentUrl,
    string? PixQrCode,
    string? PixCopyPaste,
    string? ErrorMessage
);
