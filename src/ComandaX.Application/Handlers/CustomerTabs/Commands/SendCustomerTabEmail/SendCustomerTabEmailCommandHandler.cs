using Microsoft.Extensions.Logging;
using ComandaX.Application.Constants;
using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;
using Resend;

namespace ComandaX.Application.Handlers.CustomerTabs.Commands.SendCustomerTabEmail;

public class SendCustomerTabEmailCommandHandler(
    ICustomerTabRepository tabRepository,
    IResend resend,
    ILogger<SendCustomerTabEmailCommandHandler> logger
) : IRequestHandler<SendCustomerTabEmailCommand, bool>
{
    public async Task<bool> Handle(SendCustomerTabEmailCommand request, CancellationToken cancellationToken)
    {
        var tab = await tabRepository.GetByIdWithOrdersAsync(request.CustomerTabId)
            ?? throw new RecordNotFoundException($"Customer tab with Id {request.CustomerTabId} not found.");

        var message = new EmailMessage
        {
            From = EmailConstants.FROM_EMAIL,
            To = { request.Email },
            Subject = "Sua comanda chegou - MM Sorveteria e pastelaria",
            HtmlBody = EmailConstants.GetCustomerTabEmailBody(tab)
        };

        try
        {
            await resend.EmailSendAsync(message, cancellationToken);
            return true;
        }
        catch (ResendException ex)
        {
            logger.LogError(ex, "Failed to send email: {Message}", ex.Message);
            return false;
        }
    }
}
