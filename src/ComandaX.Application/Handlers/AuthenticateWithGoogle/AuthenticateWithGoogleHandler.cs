using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ComandaX.Application.DTOs;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using Google.Apis.Auth;
using MediatR;

namespace ComandaX.Application.Handlers.AuthenticateWithGoogle;

public class AuthenticateWithGoogleHandler(IUnitOfWork _unitOfWork, IConfiguration _config) : IRequestHandler<AuthenticateWithGoogleCommand, AuthResultDto>
{
    public async Task<AuthResultDto> Handle(AuthenticateWithGoogleCommand request, CancellationToken cancellationToken)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
        var email = payload.Email;

        // Try to find an existing user by email
        var user = await _unitOfWork.Users.GetByEmailAsync(email);

        if (user == null)
        {
            // User not registered yet: create a new tenant, user and free long-lived subscription
            var tenantName = !string.IsNullOrWhiteSpace(payload.Name) ? payload.Name : email;

            var tenant = new Tenant(tenantName);
            await _unitOfWork.Tenants.AddAsync(tenant);

            // For now the product is free, so create a long-lived free subscription
            var subscription = Subscription.CreateFreeForOneYear(tenant.Id);
            await _unitOfWork.Subscriptions.AddAsync(subscription);

            // Register the user as the tenant admin
            user = new User(email, "Admin", tenant.Id);
            await _unitOfWork.Users.AddAsync(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

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
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("tenant_id", user.TenantId.ToString())
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
