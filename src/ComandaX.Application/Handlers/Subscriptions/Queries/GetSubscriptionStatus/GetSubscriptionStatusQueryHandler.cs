using Microsoft.Extensions.Logging;
using MediatR;
using ComandaX.Application.Interfaces;
using ComandaX.Application.Exceptions;

namespace ComandaX.Application.Handlers.Subscriptions.Queries.GetSubscriptionStatus;

/// <summary>
/// Handler for GetSubscriptionStatusQuery.
/// Retrieves the subscription status for a tenant.
/// </summary>
public class GetSubscriptionStatusQueryHandler : IRequestHandler<GetSubscriptionStatusQuery, SubscriptionStatusDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetSubscriptionStatusQueryHandler> _logger;

    public GetSubscriptionStatusQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetSubscriptionStatusQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<SubscriptionStatusDto> Handle(
        GetSubscriptionStatusQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting subscription status for tenant {TenantId}", request.TenantId);

        var subscription = await _unitOfWork.Subscriptions.GetByTenantIdAsync(request.TenantId)
            ?? throw new RecordNotFoundException($"Subscription not found for tenant {request.TenantId}");

        var daysRemaining = Math.Max(0, (int)(subscription.EndDate - DateTime.UtcNow).TotalDays);
        var priceInBrl = (subscription.PriceInCentavos ?? Domain.Entities.Subscription.MONTHLY_PRICE_CENTAVOS) / 100m;

        return new SubscriptionStatusDto(
            Id: subscription.Id,
            Status: subscription.Status.ToString(),
            StartDate: subscription.StartDate,
            EndDate: subscription.EndDate,
            IsValid: subscription.IsValid,
            AllowsWriteOperations: subscription.AllowsWriteOperations,
            IsExpiringSoon: subscription.IsExpiringSoon(),
            DaysRemaining: daysRemaining,
            PriceInBrl: priceInBrl
        );
    }
}
