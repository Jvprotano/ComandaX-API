using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Application;
using ComandaX.Application.Behaviors;
using ComandaX.Infrastructure;
using ComandaX.WebAPI.Extensions;
using FluentValidation;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();

builder.Services.AddGraphQLServices();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AssemblyMarker>());

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<AssemblyMarker>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddCors(options =>
{
    options.AddPolicy("RestrictedPolicy", policy =>
    {
        var allowedOrigins = builder.Configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? [];

        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
});

var app = builder.Build();

app.UseMiddleware<ComandaX.WebAPI.Middleware.ExceptionMiddleware>();

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.MapGraphQL("/graphql");

app.UseRouting();

app.MapGet("/", () => "API running ✅");

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();

    await SeedData.SeedAdminsAsync(context);

    bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
    if (isDevelopment)
        await SeedData.GenerateDevTestData(context);
}

app.Run();