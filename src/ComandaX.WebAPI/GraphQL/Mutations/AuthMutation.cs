using ComandaX.Application.DTOs;
using ComandaX.Application.Handlers.AuthenticateWithGoogle;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Mutations;

[ExtendObjectType("Mutation")]
public class AuthMutation
{
    public async Task<AuthResultDto> AuthenticateWithGoogle(
    string idToken,
    [Service] IMediator mediator)
    {
        return await mediator.Send(new AuthenticateWithGoogleCommand(idToken));
    }
}
