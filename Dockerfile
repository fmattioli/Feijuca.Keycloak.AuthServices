#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Feijuca.Keycloak.TokenManager/TokenManager.Api/TokenManager.Api.csproj", "TokenManager.Api/"]
COPY ["src/Feijuca.Keycloak.TokenManager/TokenManager.Infra.CrossCutting/TokenManager.Infra.CrossCutting.csproj", "TokenManager.Infra.CrossCutting/"]
COPY ["src/Feijuca.Keycloak.TokenManager/TokenManager.Application.Services/TokenManager.Application.Services.csproj", "TokenManager.Application.Services/"]
COPY ["src/Feijuca.Keycloak.TokenManager/TokenManager.Infra.Data/TokenManager.Infra.Data.csproj", "TokenManager.Infra.Data/"]
COPY ["src/Feijuca.Keycloak.TokenManager/TokenManager.Domain/TokenManager.Domain.csproj", "TokenManager.Domain/"]
RUN dotnet restore "TokenManager.Api/TokenManager.Api.csproj"
COPY . .

RUN dotnet build "src/Feijuca.Keycloak.TokenManager/TokenManager.Api/TokenManager.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Feijuca.Keycloak.TokenManager/TokenManager.Api/TokenManager.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TokenManager.Api.dll"]