using MediatR;

namespace CityDiscovery.ReviewService.Application.Favorites.Commands.RemoveFavorite;

public sealed class RemoveFavoriteCommand : IRequest
{
    public Guid UserId { get; init; }
    public Guid VenueId { get; init; }
}
