using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.CustomerTabs.Commands.CreateCustomerTab;

public sealed record CreateCustomerTabCommand(Guid TableId, string? Name) : IRequest<CustomerTab>;
