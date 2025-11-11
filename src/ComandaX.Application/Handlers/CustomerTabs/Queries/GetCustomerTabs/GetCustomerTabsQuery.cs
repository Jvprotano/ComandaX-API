using ComandaX.Application.DTOs;
using ComandaX.Domain.Enums;
using MediatR;

namespace ComandaX.Application.Handlers.CustomerTabs.Queries.GetCustomerTabs;

public sealed record GetCustomerTabsQuery(CustomerTabStatusEnum? Status) : IRequest<IQueryable<CustomerTabDto>>;
