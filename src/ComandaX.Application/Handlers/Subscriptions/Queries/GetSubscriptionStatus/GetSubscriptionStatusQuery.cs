using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Handlers.Subscriptions.Queries.GetSubscriptionStatus;

/// <summary>
/// Query to get the current subscription status for a tenant.
/// </summary>
public record GetSubscriptionStatusQuery(Guid TenantId) : IRequest<SubscriptionStatusDto>;

/// <summary>
/// DTO representing subscription status information.
/// </summary>
public record SubscriptionStatusDto(
    Guid Id,
    string Status,
    DateTime StartDate,
    DateTime EndDate,
    bool IsValid,
    bool AllowsWriteOperations,
    bool IsExpiringSoon,
    int DaysRemaining,
    decimal PriceInBrl
);
