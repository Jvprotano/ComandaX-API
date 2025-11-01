using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.CustomerTabs.Queries.GetCustomerTabById;

public sealed record GetCustomerTabByIdQuery(Guid Id) : IRequest<CustomerTab>;