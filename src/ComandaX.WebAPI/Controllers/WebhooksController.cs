using Microsoft.AspNetCore.Mvc;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.WebAPI.Controllers;

[ApiController]
[Route("api/webhooks")]
public class WebhooksController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAbacatePayService _abacatePayService;
    private readonly ISubscriptionNotificationService _notificationService;
    private readonly ILogger<WebhooksController> _logger;

    public WebhooksController(
        IUnitOfWork unitOfWork,
        IAbacatePayService abacatePayService,
        ISubscriptionNotificationService notificationService,
        ILogger<WebhooksController> logger)
    {
        _unitOfWork = unitOfWork;
        _abacatePayService = abacatePayService;
        _notificationService = notificationService;
        _logger = logger;
    }

    /// <summary>
    /// Webhook endpoint for AbacatePay payment notifications.
    /// </summary>
    [HttpPost("abacatepay")]
    public async Task<IActionResult> AbacatePayWebhook(
        [FromQuery] string? secret,
        [FromHeader(Name = "X-Abacatepay-Signature")] string? signature)
    {
        // Read the raw body for signature validation
        using var reader = new StreamReader(Request.Body);
        var payload = await reader.ReadToEndAsync();

        _logger.LogInformation("Received AbacatePay webhook");

        // Validate signature if configured
        if (!_abacatePayService.ValidateWebhookSignature(payload, signature ?? ""))
        {
            _logger.LogWarning("Invalid webhook signature");
            return Unauthorized("Invalid signature");
        }

        try
        {
            var webhookPayload = System.Text.Json.JsonSerializer.Deserialize<AbacatePayWebhookPayload>(
                payload,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (webhookPayload == null)
            {
                _logger.LogWarning("Failed to deserialize webhook payload");
                return BadRequest("Invalid payload");
            }

            // Handle billing.paid event
            if (webhookPayload.Event == "billing.paid")
            {
                await HandleBillingPaidAsync(webhookPayload);
            }
            else
            {
                _logger.LogInformation("Ignoring webhook event: {Event}", webhookPayload.Event);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing AbacatePay webhook");
            return StatusCode(500, "Internal server error");
        }
    }

    private async Task HandleBillingPaidAsync(AbacatePayWebhookPayload payload)
    {
        var billingData = payload.Data;
        if (billingData == null)
        {
            _logger.LogWarning("Billing data is null in webhook payload");
            return;
        }

        // Extract tenant ID from metadata
        Guid? tenantId = null;
        if (billingData.Metadata?.TenantId != null && Guid.TryParse(billingData.Metadata.TenantId, out var parsedTenantId))
        {
            tenantId = parsedTenantId;
        }

        if (tenantId == null)
        {
            _logger.LogWarning("Could not extract tenant ID from webhook metadata");
            return;
        }

        _logger.LogInformation("Processing billing.paid for tenant {TenantId}, billing {BillingId}",
            tenantId, billingData.Id);

        var tenant = await _unitOfWork.Tenants.GetByIdAsync(tenantId.Value);
        if (tenant == null)
        {
            _logger.LogWarning("Tenant {TenantId} not found", tenantId);
            return;
        }

        var subscription = await _unitOfWork.Subscriptions.GetByTenantIdAsync(tenantId.Value);

        if (subscription == null)
        {
            // Create new subscription if it doesn't exist
            subscription = new Subscription(tenantId.Value);
            await _unitOfWork.Subscriptions.AddAsync(subscription);
        }

        // Activate/renew the subscription
        subscription.Activate(
            billingData.Id ?? "",
            billingData.Customer?.Id,
            billingData.Amount);

        _unitOfWork.Subscriptions.Update(subscription);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Subscription activated for tenant {TenantId}, expires {EndDate}",
            tenantId, subscription.EndDate);

        // Send confirmation email
        try
        {
            var customerEmail = billingData.Customer?.Email ?? tenant.Name;
            await _notificationService.SendSubscriptionActivatedEmailAsync(
                tenant,
                subscription,
                customerEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send subscription activated notification for tenant {TenantId}", tenantId);
            // Don't fail the webhook if notification fails
        }
    }
}

