namespace ComandaX.Application.Interfaces;

/// <summary>
/// Service interface for AbacatePay payment integration.
/// </summary>
public interface IAbacatePayService
{
    /// <summary>
    /// Creates a new billing (PIX payment) for subscription.
    /// </summary>
    /// <param name="tenantId">The tenant ID for reference.</param>
    /// <param name="customerEmail">Customer's email address.</param>
    /// <param name="customerName">Customer's name.</param>
    /// <param name="amountInCentavos">Amount in centavos (e.g., 4990 for R$49.90).</param>
    /// <returns>The billing creation result with payment URL and QR code.</returns>
    Task<AbacatePayBillingResult> CreateBillingAsync(
        Guid tenantId,
        string customerEmail,
        string customerName,
        int amountInCentavos);

    /// <summary>
    /// Validates a webhook signature from AbacatePay.
    /// </summary>
    /// <param name="payload">The raw webhook payload.</param>
    /// <param name="signature">The signature from the header.</param>
    /// <returns>True if the signature is valid.</returns>
    bool ValidateWebhookSignature(string payload, string signature);
}

/// <summary>
/// Result of creating a billing in AbacatePay.
/// </summary>
public record AbacatePayBillingResult(
    bool Success,
    string? BillingId,
    string? PaymentUrl,
    string? PixQrCode,
    string? PixCopyPaste,
    string? CustomerId,
    string? ErrorMessage);

/// <summary>
/// Webhook payload from AbacatePay for billing.paid event.
/// </summary>
public record AbacatePayWebhookPayload(
    string Id,
    string Event,
    bool DevMode,
    AbacatePayBillingData Data);

/// <summary>
/// Billing data from AbacatePay webhook.
/// </summary>
public record AbacatePayBillingData(
    string Id,
    string Url,
    int Amount,
    int Fee,
    string Status,
    AbacatePayCustomerData Customer,
    AbacatePayProductData[] Products,
    AbacatePayMetadata? Metadata);

/// <summary>
/// Customer data from AbacatePay.
/// </summary>
public record AbacatePayCustomerData(
    string Id,
    string Email,
    string Name);

/// <summary>
/// Product data from AbacatePay.
/// </summary>
public record AbacatePayProductData(
    string Id,
    string Name,
    int Quantity,
    int Price);

/// <summary>
/// Metadata from AbacatePay billing.
/// </summary>
public record AbacatePayMetadata(
    string? TenantId);

