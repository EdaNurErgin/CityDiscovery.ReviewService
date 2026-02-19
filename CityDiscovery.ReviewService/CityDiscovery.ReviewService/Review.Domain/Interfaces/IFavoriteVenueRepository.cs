using CityDiscovery.ReviewService.Domain.Entities;

namespace CityDiscovery.ReviewService.Domain.Interfaces;

public interface IFavoriteVenueRepository
{
    Task<bool> ExistsAsync(Guid userId, Guid venueId, CancellationToken cancellationToken = default);
    Task<List<FavoriteVenue>> GetUserFavoritesAsync(Guid userId, CancellationToken cancellationToken = default);

    Task AddAsync(FavoriteVenue favorite, CancellationToken cancellationToken = default);
    Task RemoveAsync(Guid userId, Guid venueId, CancellationToken cancellationToken = default);
    Task DeleteFavoritesByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
