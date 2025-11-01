namespace ComandaX.Application.DTOs;

public sealed record CustomerTabDto(
    Guid Id,
    string Name,
    Guid? TableId,
    TableDto? Table);
