using ComandaX.Domain.Enums;

namespace ComandaX.Application.DTOs;

public record TableDto(Guid Id, int? Number, TableStatusEnum Status);
