using ComandaX.Application.Handlers.Subscriptions.Commands.InitiatePayment;
using ComandaX.Application.Handlers.Subscriptions.Queries.GetSubscriptionStatus;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Mutations;

[ExtendObjectType("Mutation")]
public class SubscriptionMutation
{
    /// <summary>
    /// Initiates a payment for subscription renewal.
    /// Returns payment details including PIX QR code and payment URL.
    /// </summary>
    public async Task<InitiatePaymentResult> InitiateSubscriptionPaymentAsync(
        Guid tenantId,
        string customerEmail,
        string customerName,
        [Service] ISender mediator)
    {
        return await mediator.Send(new InitiatePaymentCommand(tenantId, customerEmail, customerName));
    }
}
