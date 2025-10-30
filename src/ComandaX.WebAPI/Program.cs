using Application;
using ComandaX.Application.Interfaces;
using ComandaX.Infrastructure.Persistence;
using ComandaX.Infrastructure.Persistence.Repository;
using ComandaX.WebAPI.GraphQL.Mutations;
using ComandaX.WebAPI.GraphQL.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddSingleton<DbFake>();

builder.Services.AddGraphQLServer()
            .AddQueryType(d => d.Name("Query"))
            .AddType<ProductQuery>()
            .AddMutationType(d => d.Name("Mutation"))
            .AddType<ProductMutation>()
            .AddProjections()
            .AddFiltering()
            .AddSorting();
;

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

app.Run();