using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.CustomerTabs.Queries.GetCustomerTabs;

public sealed record GetCustomerTabsQuery : IRequest<IQueryable<CustomerTab>>;
