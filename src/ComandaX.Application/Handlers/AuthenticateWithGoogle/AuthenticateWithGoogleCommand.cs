using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Handlers.AuthenticateWithGoogle;

public sealed record AuthenticateWithGoogleCommand(string IdToken) : IRequest<AuthResultDto>;