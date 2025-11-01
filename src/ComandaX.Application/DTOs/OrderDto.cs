using ComandaX.Domain.Enums;

namespace ComandaX.Application.DTOs;

public sealed record OrderDto(
    int Code,
    List<OrderProductDto> Products,
    OrderStatusEnum Status);
