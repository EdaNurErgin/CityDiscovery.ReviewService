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
using CityDiscovery.ReviewService.Infrastructure.MessageBus.Consumers;
using MassTransit;

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
        // --- MASSTRANSIT AYARLARI ---
        services.AddMassTransit(x =>
        {
            // Yeni namespace altındaki Consumer'ı ekliyoruz
            x.AddConsumer<VenueDeletedConsumer>();
            x.AddConsumer<UserDeletedConsumer>();
            x.AddConsumer<UserUpdatedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var host = configuration["RabbitMQ:Host"] ?? "localhost";
                var user = configuration["RabbitMQ:Username"] ?? "guest";
                var pass = configuration["RabbitMQ:Password"] ?? "guest";

                cfg.Host(host, h =>
                {
                    h.Username(user);
                    h.Password(pass);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
