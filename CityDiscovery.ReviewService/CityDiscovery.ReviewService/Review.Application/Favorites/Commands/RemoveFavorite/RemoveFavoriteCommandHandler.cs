using CityDiscovery.ReviewService.Domain.Interfaces;
using MediatR;

namespace CityDiscovery.ReviewService.Application.Favorites.Commands.RemoveFavorite;

public sealed class RemoveFavoriteCommandHandler : IRequestHandler<RemoveFavoriteCommand>
{
    private readonly IFavoriteVenueRepository _favoriteRepository;

    public RemoveFavoriteCommandHandler(IFavoriteVenueRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }

    public async Task Handle(RemoveFavoriteCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
            throw new InvalidOperationException("UserId is required.");

        await _favoriteRepository.RemoveAsync(request.UserId, request.VenueId, cancellationToken);
        // Task return type → ekstra return ifadesine gerek yok
    }
}
