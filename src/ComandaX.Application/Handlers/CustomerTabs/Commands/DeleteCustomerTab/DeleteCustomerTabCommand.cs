using MediatR;

namespace ComandaX.Application.Handlers.CustomerTabs.Commands.DeleteCustomerTab;

public sealed record DeleteCustomerTabCommand(Guid Id) : IRequest;

