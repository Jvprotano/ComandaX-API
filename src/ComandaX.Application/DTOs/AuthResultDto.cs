namespace ComandaX.Application.DTOs;

public sealed record AuthResultDto(string JwtToken, string Email, string Role);