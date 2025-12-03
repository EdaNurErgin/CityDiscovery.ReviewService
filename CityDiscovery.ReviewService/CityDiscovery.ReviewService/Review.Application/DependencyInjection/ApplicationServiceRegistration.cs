using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CityDiscovery.ReviewService.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddReviewApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // MediatR handlerlarını bu assembly'den tara
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });



        return services;
    }
}
