using ComandaX.Application.DTOs;
using ComandaX.Domain.Entities;

namespace ComandaX.Application.Extensions;

public static class OrderProductExtension
{
    public static OrderProductDto AsDto(this OrderProduct orderProduct)
    {
        return new OrderProductDto(
            orderProduct.ProductId,
            orderProduct.OrderId,
            orderProduct.Quantity,
            orderProduct.TotalPrice.AsMoney());
    }
}
