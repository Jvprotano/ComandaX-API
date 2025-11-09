using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.CustomerTabs.Commands.CreateCustomerTab;

public sealed record CreateCustomerTabCommand(string Name, Guid? TableId) : IRequest<CustomerTab>;
