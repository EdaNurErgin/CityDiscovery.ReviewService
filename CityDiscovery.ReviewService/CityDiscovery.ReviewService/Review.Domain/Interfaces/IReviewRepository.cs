using CityDiscovery.ReviewService.Domain.Entities;
namespace CityDiscovery.ReviewService.Domain.Interfaces;

public interface IReviewRepository
{
    Task<Reviewx?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Reviewx?> GetByUserAndVenueAsync(Guid userId, Guid venueId, CancellationToken cancellationToken = default);

    Task<List<Reviewx>> GetVenueReviewsAsync(Guid venueId, CancellationToken cancellationToken = default);

    Task AddAsync(Reviewx review, CancellationToken cancellationToken = default);
    Task UpdateAsync(Reviewx review, CancellationToken cancellationToken = default);
}
