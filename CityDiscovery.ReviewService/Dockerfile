

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["CityDiscovery.ReviewService/CityDiscovery.ReviewService.csproj", "CityDiscovery.ReviewService/"]
RUN dotnet restore "CityDiscovery.ReviewService/CityDiscovery.ReviewService.csproj"

COPY . .


WORKDIR "/src/CityDiscovery.ReviewService"
RUN dotnet build "CityDiscovery.ReviewService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CityDiscovery.ReviewService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CityDiscovery.ReviewService.dll"]