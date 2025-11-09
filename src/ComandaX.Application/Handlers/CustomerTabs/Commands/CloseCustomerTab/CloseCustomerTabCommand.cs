using MediatR;

namespace ComandaX.Application.Handlers.CustomerTabs.Commands.CloseCustomerTab;

public sealed record CloseCustomerTabCommand(Guid CustomerTabId) : IRequest<Unit>;

