using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ComandaX.Application.Interfaces;

namespace ComandaX.Infrastructure.Services;

/// <summary>
/// Service for integrating with AbacatePay payment API.
/// </summary>
public class AbacatePayService : IAbacatePayService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AbacatePayService> _logger;
    private readonly string _apiKey;
    private readonly string _webhookSecret;
    private const string BaseUrl = "https://api.abacatepay.com";

    public AbacatePayService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<AbacatePayService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = configuration["AbacatePay:ApiKey"] ?? throw new ArgumentNullException("AbacatePay:ApiKey configuration is required");
        _webhookSecret = configuration["AbacatePay:WebhookSecret"] ?? "";

        _httpClient.BaseAddress = new Uri(BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    public async Task<AbacatePayBillingResult> CreateBillingAsync(
        Guid tenantId,
        string customerEmail,
        string customerName,
        int amountInCentavos)
    {
        try
        {
            var request = new
            {
                frequency = "ONE_TIME",
                methods = new[] { "PIX" },
                products = new[]
                {
                    new
                    {
                        externalId = tenantId.ToString(),
                        name = "Assinatura Mensal ComandaX",
                        quantity = 1,
                        price = amountInCentavos
                    }
                },
                returnUrl = "https://comandax.app/subscription/success",
                completionUrl = "https://comandax.app/subscription/complete",
                customer = new
                {
                    email = customerEmail,
                    name = customerName
                },
                metadata = new
                {
                    tenantId = tenantId.ToString()
                }
            };

            var response = await _httpClient.PostAsJsonAsync("/v1/billing/create", request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("AbacatePay billing creation failed: {StatusCode} - {Error}",
                    response.StatusCode, errorContent);
                return new AbacatePayBillingResult(
                    Success: false,
                    BillingId: null,
                    PaymentUrl: null,
                    PixQrCode: null,
                    PixCopyPaste: null,
                    CustomerId: null,
                    ErrorMessage: $"API Error: {response.StatusCode}");
            }

            var result = await response.Content.ReadFromJsonAsync<AbacatePayCreateBillingResponse>();

            if (result?.Data == null)
            {
                return new AbacatePayBillingResult(
                    Success: false,
                    BillingId: null,
                    PaymentUrl: null,
                    PixQrCode: null,
                    PixCopyPaste: null,
                    CustomerId: null,
                    ErrorMessage: "Invalid response from AbacatePay");
            }

            return new AbacatePayBillingResult(
                Success: true,
                BillingId: result.Data.Id,
                PaymentUrl: result.Data.Url,
                PixQrCode: result.Data.Pix?.QrCode,
                PixCopyPaste: result.Data.Pix?.CopyPaste,
                CustomerId: result.Data.Customer?.Id,
                ErrorMessage: null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating AbacatePay billing for tenant {TenantId}", tenantId);
            return new AbacatePayBillingResult(
                Success: false,
                BillingId: null,
                PaymentUrl: null,
                PixQrCode: null,
                PixCopyPaste: null,
                CustomerId: null,
                ErrorMessage: ex.Message);
        }
    }

    public bool ValidateWebhookSignature(string payload, string signature)
    {
        if (string.IsNullOrEmpty(_webhookSecret))
        {
            _logger.LogWarning("Webhook secret not configured, skipping signature validation");
            return true;
        }

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_webhookSecret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        var computedSignature = Convert.ToHexString(hash).ToLowerInvariant();

        return computedSignature == signature?.ToLowerInvariant();
    }
}

// Response DTOs for AbacatePay API
internal class AbacatePayCreateBillingResponse
{
    [JsonPropertyName("data")]
    public AbacatePayBillingResponseData? Data { get; set; }
}

internal class AbacatePayBillingResponseData
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("pix")]
    public AbacatePayPixData? Pix { get; set; }

    [JsonPropertyName("customer")]
    public AbacatePayCustomerResponseData? Customer { get; set; }
}

internal class AbacatePayPixData
{
    [JsonPropertyName("qrCode")]
    public string? QrCode { get; set; }

    [JsonPropertyName("copyPaste")]
    public string? CopyPaste { get; set; }
}

internal class AbacatePayCustomerResponseData
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

