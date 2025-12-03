using CityDiscovery.ReviewService.Application.DTOs;
using CityDiscovery.ReviewService.Domain.Interfaces;
using MediatR;

namespace CityDiscovery.ReviewService.Application.Favorites.Queries.GetUserFavorites;

public sealed class GetUserFavoritesQueryHandler
    : IRequestHandler<GetUserFavoritesQuery, List<FavoriteVenueDto>>
{
    private readonly IFavoriteVenueRepository _favoriteRepository;

    public GetUserFavoritesQueryHandler(IFavoriteVenueRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }

    public async Task<List<FavoriteVenueDto>> Handle(GetUserFavoritesQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
            throw new InvalidOperationException("UserId is required.");

        var favorites = await _favoriteRepository.GetUserFavoritesAsync(request.UserId, cancellationToken);

        return favorites
            .Select(f => new FavoriteVenueDto
            {
                Id = f.Id,
                UserId = f.UserId,
                VenueId = f.VenueId,
                CreatedAt = f.CreatedAt
            })
            .ToList();
    }
}
