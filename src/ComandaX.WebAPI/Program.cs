using Microsoft.EntityFrameworkCore;
using Application;
using ComandaX.Infrastructure;
using ComandaX.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();

builder.Services.AddGraphQLServices();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AssemblyMarker>());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseMiddleware<ComandaX.WebAPI.Middleware.ExceptionMiddleware>();

app.UseHttpsRedirection();
app.MapGraphQL("/graphql");

app.UseCors("AllowAll");
app.UseRouting();

app.MapGet("/", () => "API running ✅");

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
db.Database.Migrate();

app.Run();