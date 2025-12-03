using CityDiscovery.ReviewService.Application.DTOs;
using MediatR;

namespace CityDiscovery.ReviewService.Application.Favorites.Queries.GetUserFavorites;

public sealed class GetUserFavoritesQuery : IRequest<List<FavoriteVenueDto>>
{
    public Guid UserId { get; init; }
}
