using MediatR;

namespace ComandaX.Application.Handlers.CustomerTabs.Commands.SendCustomerTabEmail;

public record SendCustomerTabEmailCommand(Guid CustomerTabId, string Email) : IRequest<bool>;
