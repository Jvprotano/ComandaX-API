using ComandaX.Domain.Entities;

namespace ComandaX.Application.DTOs;

public sealed record TabDto(
    string Name,
    Guid? TableId,
    TableDto Table);
