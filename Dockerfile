FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["CampanhasApi.Api/CampanhasApi.Api.csproj", "CampanhasApi.Api/"]
COPY ["CampanhasApi.Application/CampanhasApi.Application.csproj", "CampanhasApi.Application/"]
COPY ["CampanhasApi.Domain/CampanhasApi.Domain.csproj", "CampanhasApi.Domain/"]
COPY ["CampanhasApi.Infrastructure/CampanhasApi.Infrastructure.csproj", "CampanhasApi.Infrastructure/"]
RUN dotnet restore "CampanhasApi.Api/CampanhasApi.Api.csproj"
COPY . .
WORKDIR "/src/CampanhasApi.Api"
RUN dotnet build "CampanhasApi.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CampanhasApi.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CampanhasApi.Api.dll"]
