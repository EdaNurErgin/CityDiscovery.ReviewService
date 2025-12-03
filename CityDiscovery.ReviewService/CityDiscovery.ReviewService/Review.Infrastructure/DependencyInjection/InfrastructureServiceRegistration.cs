using CityDiscovery.ReviewService.Application.Interfaces;
using CityDiscovery.ReviewService.Domain.Interfaces;
using CityDiscovery.ReviewService.Infrastructure.Data;
using CityDiscovery.ReviewService.Infrastructure.ExternalServices;
using CityDiscovery.ReviewService.Infrastructure.Security;
using CityDiscovery.ReviewService.Review.Application.Interfaces;
using CityDiscovery.ReviewService.Review.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CityDiscovery.ReviewService.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddReviewInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ReviewDbContext>(options =>
        {
            options.UseSqlServer(
                connectionString,
                sql =>
                {
                    sql.MigrationsAssembly(typeof(ReviewDbContext).Assembly.FullName);
                });
        });

        // Repositories
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IFavoriteVenueRepository, FavoriteVenueRepository>();
        services.AddJwtAuthentication(configuration);
        // External Services (HTTP Clients)
        services.AddHttpClient<IIdentityServiceClient, IdentityServiceClient>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(configuration["Services:Identity"]);
            });

        services.AddHttpClient<IVenueServiceClient, VenueServiceClient>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(configuration["Services:Venue"]);
            });

        return services;
    }
}
