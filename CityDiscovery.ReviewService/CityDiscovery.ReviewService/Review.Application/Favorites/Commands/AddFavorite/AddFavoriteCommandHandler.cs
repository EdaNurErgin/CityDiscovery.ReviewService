using CityDiscovery.ReviewService.Application.Interfaces;
using CityDiscovery.ReviewService.Domain.Entities;
using CityDiscovery.ReviewService.Domain.Interfaces;
using CityDiscovery.ReviewService.Review.Application.Interfaces;
using MediatR;
using MassTransit;
using CityDiscovery.ReviewService.ReviewService.Shared.Events.Venue;

namespace CityDiscovery.ReviewService.Application.Favorites.Commands.AddFavorite;

public sealed class AddFavoriteCommandHandler : IRequestHandler<AddFavoriteCommand>
{
    private readonly IFavoriteVenueRepository _favoriteRepository;
    private readonly IIdentityServiceClient _identityClient;
    private readonly IVenueServiceClient _venueClient;
    private readonly IPublishEndpoint _publishEndpoint;

    public AddFavoriteCommandHandler(
        IFavoriteVenueRepository favoriteRepository,
        IIdentityServiceClient identityClient,
        IVenueServiceClient venueClient,
        IPublishEndpoint publishEndpoint)
    {
        _favoriteRepository = favoriteRepository;
        _identityClient = identityClient;
        _venueClient = venueClient;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
            throw new InvalidOperationException("UserId is required.");

        // Kullanıcı var mı?
        var userExists = await _identityClient.CheckUserExistsAsync(request.UserId, cancellationToken);
        if (!userExists)
            throw new InvalidOperationException("User not found.");

        // Mekan var mı?
        var venueExists = await _venueClient.CheckVenueExistsAsync(request.VenueId, cancellationToken);
        if (!venueExists)
            throw new InvalidOperationException("Venue not found.");

        // Zaten favori mi?
        var exists = await _favoriteRepository.ExistsAsync(request.UserId, request.VenueId, cancellationToken);
        if (exists)
            return; 

        var favorite = new FavoriteVenue(request.UserId, request.VenueId);
        await _favoriteRepository.AddAsync(favorite, cancellationToken);
        // Mekan favorilendiğinde diğer servislere haber ver
        await _publishEndpoint.Publish(new VenueFavoritedEvent
        {
            VenueId = request.VenueId,
            UserId = request.UserId,
            FavoritedAt = DateTime.UtcNow // Eğer Event sınıfında bu alan varsa
        }, cancellationToken);
    }
}
