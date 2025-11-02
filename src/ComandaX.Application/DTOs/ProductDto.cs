namespace ComandaX.Application.DTOs;

public sealed record ProductDto(
    Guid Id,
    string Name,
    decimal Price,
    int Code,
    bool NeedPreparation);