using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.CustomerTabs.Commands.CreateCustomerTab;

public sealed record CreateCustomerTabCommand(Guid TableId, string? Name) : IRequest<CustomerTab>;
