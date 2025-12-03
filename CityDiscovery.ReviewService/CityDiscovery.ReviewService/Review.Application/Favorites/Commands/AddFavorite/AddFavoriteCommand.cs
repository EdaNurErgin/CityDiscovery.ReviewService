using MediatR;

namespace CityDiscovery.ReviewService.Application.Favorites.Commands.AddFavorite;

public sealed class AddFavoriteCommand : IRequest
{
    public Guid UserId { get; init; }
    public Guid VenueId { get; init; }
}
