using ComandaX.Domain.Enums;

namespace ComandaX.Application.DTOs;

public sealed record OrderDto(
    Guid Id,
    int Code,
    Guid? CustomerTabId,
    List<OrderProductDto> Products,
    OrderStatusEnum Status);
