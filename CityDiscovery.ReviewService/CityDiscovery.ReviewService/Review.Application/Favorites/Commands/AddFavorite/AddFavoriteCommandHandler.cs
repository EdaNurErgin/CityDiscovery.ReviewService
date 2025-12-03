using CityDiscovery.ReviewService.Application.Interfaces;
using CityDiscovery.ReviewService.Domain.Entities;
using CityDiscovery.ReviewService.Domain.Interfaces;
using CityDiscovery.ReviewService.Review.Application.Interfaces;
using MediatR;

namespace CityDiscovery.ReviewService.Application.Favorites.Commands.AddFavorite;

public sealed class AddFavoriteCommandHandler : IRequestHandler<AddFavoriteCommand>
{
    private readonly IFavoriteVenueRepository _favoriteRepository;
    private readonly IIdentityServiceClient _identityClient;
    private readonly IVenueServiceClient _venueClient;

    public AddFavoriteCommandHandler(
        IFavoriteVenueRepository favoriteRepository,
        IIdentityServiceClient identityClient,
        IVenueServiceClient venueClient)
    {
        _favoriteRepository = favoriteRepository;
        _identityClient = identityClient;
        _venueClient = venueClient;
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
            return; // Sessizce dön, ekstra exception atmaya gerek yok

        var favorite = new FavoriteVenue(request.UserId, request.VenueId);
        await _favoriteRepository.AddAsync(favorite, cancellationToken);

        // Task dönüyoruz, Unit return yok
    }
}
