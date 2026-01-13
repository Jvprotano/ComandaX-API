using System.Security.Claims;
using System.Text.Json;
using ComandaX.Application.Interfaces;

namespace ComandaX.WebAPI.Middleware;

/// <summary>
/// Middleware that checks the tenant's subscription status and blocks write operations
/// (GraphQL mutations) if the subscription has expired.
/// </summary>
public class SubscriptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SubscriptionMiddleware> _logger;

    public SubscriptionMiddleware(RequestDelegate next, ILogger<SubscriptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork, ITenantService tenantService)
    {
        // Only check for GraphQL requests
        if (!context.Request.Path.StartsWithSegments("/graphql"))
        {
            await _next(context);
            return;
        }

        // Only check for authenticated users
        if (context.User.Identity?.IsAuthenticated != true)
        {
            await _next(context);
            return;
        }

        var tenantId = tenantService.GetCurrentTenantId();
        if (tenantId == null || tenantId == Guid.Empty)
        {
            await _next(context);
            return;
        }

        // Check if this is a mutation (write operation)
        var isMutation = await IsMutationRequestAsync(context);
        if (!isMutation)
        {
            // Read operations are always allowed
            await _next(context);
            return;
        }

        // Check subscription status
        var subscription = await unitOfWork.Subscriptions.GetByTenantIdAsync(tenantId.Value);
        
        if (subscription == null)
        {
            _logger.LogWarning("Tenant {TenantId} has no subscription", tenantId);
            await WriteSubscriptionRequiredResponse(context);
            return;
        }

        if (!subscription.AllowsWriteOperations)
        {
            _logger.LogInformation("Tenant {TenantId} subscription expired, blocking mutation", tenantId);
            await WriteSubscriptionExpiredResponse(context);
            return;
        }

        await _next(context);
    }

    private static async Task<bool> IsMutationRequestAsync(HttpContext context)
    {
        // Only POST requests can be mutations
        if (context.Request.Method != "POST")
            return false;

        // Enable buffering to read the body multiple times
        context.Request.EnableBuffering();

        try
        {
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (string.IsNullOrWhiteSpace(body))
                return false;

            // Parse the GraphQL request to check if it's a mutation
            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("query", out var queryElement))
            {
                var query = queryElement.GetString();
                if (!string.IsNullOrEmpty(query))
                {
                    // Simple check: if the query starts with "mutation" it's a mutation
                    var trimmedQuery = query.TrimStart();
                    return trimmedQuery.StartsWith("mutation", StringComparison.OrdinalIgnoreCase);
                }
            }
        }
        catch (Exception)
        {
            // If we can't parse the request, assume it's not a mutation
        }

        return false;
    }

    private static async Task WriteSubscriptionRequiredResponse(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status402PaymentRequired;
        context.Response.ContentType = "application/json";
        
        var response = new
        {
            errors = new[]
            {
                new
                {
                    message = "Subscription required. Please subscribe to access this feature.",
                    extensions = new { code = "SUBSCRIPTION_REQUIRED" }
                }
            }
        };

        await context.Response.WriteAsJsonAsync(response);
    }

    private static async Task WriteSubscriptionExpiredResponse(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status402PaymentRequired;
        context.Response.ContentType = "application/json";
        
        var response = new
        {
            errors = new[]
            {
                new
                {
                    message = "Your subscription has expired. Please renew to continue using write operations.",
                    extensions = new { code = "SUBSCRIPTION_EXPIRED" }
                }
            }
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}

/// <summary>
/// Extension methods for registering the SubscriptionMiddleware.
/// </summary>
public static class SubscriptionMiddlewareExtensions
{
    public static IApplicationBuilder UseSubscriptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SubscriptionMiddleware>();
    }
}

