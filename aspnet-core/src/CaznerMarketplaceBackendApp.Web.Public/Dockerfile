#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/CaznerMarketplaceBackendApp.Web.Public/CaznerMarketplaceBackendApp.Web.Public.csproj", "src/CaznerMarketplaceBackendApp.Web.Public/"]
COPY ["src/CaznerMarketplaceBackendApp.Web.Core/CaznerMarketplaceBackendApp.Web.Core.csproj", "src/CaznerMarketplaceBackendApp.Web.Core/"]
COPY ["src/CaznerMarketplaceBackendApp.Application/CaznerMarketplaceBackendApp.Application.csproj", "src/CaznerMarketplaceBackendApp.Application/"]
COPY ["src/CaznerMarketplaceBackendApp.Application.Shared/CaznerMarketplaceBackendApp.Application.Shared.csproj", "src/CaznerMarketplaceBackendApp.Application.Shared/"]
COPY ["src/CaznerMarketplaceBackendApp.Core.Shared/CaznerMarketplaceBackendApp.Core.Shared.csproj", "src/CaznerMarketplaceBackendApp.Core.Shared/"]
COPY ["src/CaznerMarketplaceBackendApp.Core/CaznerMarketplaceBackendApp.Core.csproj", "src/CaznerMarketplaceBackendApp.Core/"]
COPY ["src/CaznerMarketplaceBackendApp.EntityFrameworkCore/CaznerMarketplaceBackendApp.EntityFrameworkCore.csproj", "src/CaznerMarketplaceBackendApp.EntityFrameworkCore/"]
COPY ["src/CaznerMarketplaceBackendApp.GraphQL/CaznerMarketplaceBackendApp.GraphQL.csproj", "src/CaznerMarketplaceBackendApp.GraphQL/"]
RUN dotnet restore "src/CaznerMarketplaceBackendApp.Web.Public/CaznerMarketplaceBackendApp.Web.Public.csproj"
COPY . .
WORKDIR "/src/src/CaznerMarketplaceBackendApp.Web.Public"
RUN dotnet build "CaznerMarketplaceBackendApp.Web.Public.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CaznerMarketplaceBackendApp.Web.Public.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CaznerMarketplaceBackendApp.Web.Public.dll"]
