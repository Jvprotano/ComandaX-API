using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ComandaX.Application.DTOs;
using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using Google.Apis.Auth;
using MediatR;

namespace ComandaX.Application.Handlers.AuthenticateWithGoogle;

public class AuthenticateWithGoogleHandler(IUserRepository _userRepository, IConfiguration _config) : IRequestHandler<AuthenticateWithGoogleCommand, AuthResultDto>
{
    public async Task<AuthResultDto> Handle(AuthenticateWithGoogleCommand request, CancellationToken cancellationToken)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
        var email = payload.Email;

        var user = await _userRepository.GetByEmailAsync(email)
         ?? throw new UserNotAuthorizedException(email); ;

        var jwtToken = GenerateJwt(user);

        return new AuthResultDto(jwtToken, user.Email, user.Role);
    }

    private string GenerateJwt(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
