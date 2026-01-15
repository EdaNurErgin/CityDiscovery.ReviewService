using CityDiscovery.ReviewService.Domain.Entities;
namespace CityDiscovery.ReviewService.Domain.Interfaces;

public interface IReviewRepository
{
    Task<Reviewx?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Reviewx?> GetByUserAndVenueAsync(Guid userId, Guid venueId, CancellationToken cancellationToken = default);

    Task<List<Reviewx>> GetVenueReviewsAsync(Guid venueId, CancellationToken cancellationToken = default);

    Task AddAsync(Reviewx review, CancellationToken cancellationToken = default);
    Task UpdateAsync(Reviewx review, CancellationToken cancellationToken = default);
    Task UpdateReviewerDetailsAsync(Guid userId, string newUserName, string newAvatarUrl);
    void Remove(Reviewx review); // Veya Reviewx, entity adın neyse
    Task<(double AverageRating, int ReviewCount)> GetVenueRatingStatsAsync(Guid venueId, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
