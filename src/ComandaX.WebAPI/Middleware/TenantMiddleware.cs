using System.Security.Claims;
using ComandaX.Application.Interfaces;

namespace ComandaX.WebAPI.Middleware;

/// <summary>
/// Middleware that extracts the tenant ID from the authenticated user's claims
/// and sets it in the tenant service for use throughout the request.
/// </summary>
public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tenantIdClaim = context.User.FindFirst("tenant_id") 
                ?? context.User.FindFirst("TenantId")
                ?? context.User.FindFirst(ClaimTypes.GroupSid);

            if (tenantIdClaim != null && Guid.TryParse(tenantIdClaim.Value, out var tenantId))
            {
                tenantService.SetCurrentTenantId(tenantId);
            }
        }

        await _next(context);
    }
}

/// <summary>
/// Extension methods for registering the TenantMiddleware.
/// </summary>
public static class TenantMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TenantMiddleware>();
    }
}

