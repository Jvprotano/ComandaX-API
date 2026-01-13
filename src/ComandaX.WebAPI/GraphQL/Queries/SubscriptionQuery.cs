using ComandaX.Application.Handlers.Subscriptions.Queries.GetSubscriptionStatus;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Queries;

[ExtendObjectType("Query")]
public class SubscriptionQuery
{
    /// <summary>
    /// Gets the subscription status for a tenant.
    /// </summary>
    public async Task<SubscriptionStatusDto> GetSubscriptionStatusAsync(
        Guid tenantId,
        [Service] ISender mediator)
    {
        return await mediator.Send(new GetSubscriptionStatusQuery(tenantId));
    }
}
