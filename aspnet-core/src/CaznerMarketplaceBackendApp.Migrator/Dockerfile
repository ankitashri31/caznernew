#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/CaznerMarketplaceBackendApp.Migrator/CaznerMarketplaceBackendApp.Migrator.csproj", "src/CaznerMarketplaceBackendApp.Migrator/"]
COPY ["src/CaznerMarketplaceBackendApp.EntityFrameworkCore/CaznerMarketplaceBackendApp.EntityFrameworkCore.csproj", "src/CaznerMarketplaceBackendApp.EntityFrameworkCore/"]
COPY ["src/CaznerMarketplaceBackendApp.Core/CaznerMarketplaceBackendApp.Core.csproj", "src/CaznerMarketplaceBackendApp.Core/"]
COPY ["src/CaznerMarketplaceBackendApp.Core.Shared/CaznerMarketplaceBackendApp.Core.Shared.csproj", "src/CaznerMarketplaceBackendApp.Core.Shared/"]
RUN dotnet restore "src/CaznerMarketplaceBackendApp.Migrator/CaznerMarketplaceBackendApp.Migrator.csproj"
COPY . .
WORKDIR "/src/src/CaznerMarketplaceBackendApp.Migrator"
RUN dotnet build "CaznerMarketplaceBackendApp.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CaznerMarketplaceBackendApp.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CaznerMarketplaceBackendApp.Migrator.dll"]
