FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and project files
COPY src/*.sln .
COPY src/ComandaX.Application/*.csproj ./src/ComandaX.Application/
COPY src/ComandaX.Domain/*.csproj ./src/ComandaX.Domain/
COPY src/ComandaX.Infrastructure/*.csproj ./src/ComandaX.Infrastructure/
COPY src/ComandaX.WebAPI/*.csproj ./src/ComandaX.WebAPI/
COPY src/ComandaX.Tests/*.csproj ./src/ComandaX.Tests/

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY src/. ./src/

# Publish the application
RUN dotnet publish src/ComandaX.WebAPI/ComandaX.WebAPI.csproj -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ComandaX.WebAPI.dll"]
