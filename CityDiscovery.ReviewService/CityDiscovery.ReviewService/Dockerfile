# ============================================================================
# CityDiscovery Review Service - Multi-stage Dockerfile
# ============================================================================

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CityDiscovery.ReviewService.csproj", "./"]
RUN dotnet restore "CityDiscovery.ReviewService.csproj"

COPY . .
RUN dotnet build "CityDiscovery.ReviewService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CityDiscovery.ReviewService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CityDiscovery.ReviewService.dll"]
