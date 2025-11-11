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
using Resend;
using ComandaX.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();
builder.Services.AddGraphQLServices();

builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddUserSecrets<Program>();

// Register Pooled DbContextFactory for all database operations
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddPooledDbContextFactory<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register scoped DbContext for migrations and seeding (created from factory)
builder.Services.AddScoped(sp =>
{
    var factory = sp.GetRequiredService<IDbContextFactory<AppDbContext>>();
    return factory.CreateDbContext();
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AssemblyMarker>());
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

builder.Services.AddHttpClient<ResendClient>();
builder.Services.Configure<ResendClientOptions>(o =>
{
    o.ApiToken = builder.Configuration["RESEND_APITOKEN"]!;
});
builder.Services.AddTransient<IResend, ResendClient>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors("RestrictedPolicy");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL("/graphql");

app.MapGet("/", () => "API running ✅");

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
