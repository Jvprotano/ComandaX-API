FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY src/*.sln ./
COPY src/ComandaX.Application/*.csproj ComandaX.Application/
COPY src/ComandaX.Domain/*.csproj ComandaX.Domain/
COPY src/ComandaX.Infrastructure/*.csproj ComandaX.Infrastructure/
COPY src/ComandaX.WebAPI/*.csproj ComandaX.WebAPI/
COPY src/ComandaX.Tests/*.csproj ComandaX.Tests/

RUN dotnet restore

COPY src/. ./

RUN dotnet publish ComandaX.WebAPI/ComandaX.WebAPI.csproj -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ComandaX.WebAPI.dll"]
