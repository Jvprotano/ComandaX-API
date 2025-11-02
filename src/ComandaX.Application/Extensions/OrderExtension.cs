using ComandaX.Application.DTOs;
using ComandaX.Domain.Entities;

namespace ComandaX.Application.Extensions;

public static class OrderExtension
{
    public static OrderDto AsDto(this Order order)
    {
        return new OrderDto(
            order.Id,
            order.Code,
            new(),
            order.Status
        );
    }
}
