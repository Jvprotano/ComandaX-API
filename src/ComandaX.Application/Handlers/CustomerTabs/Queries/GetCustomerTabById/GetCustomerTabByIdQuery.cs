using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.CustomerTabs.Queries.GetCustomerTabById;

public sealed record GetCustomerTabByIdQuery(Guid Id) : IRequest<CustomerTab>;