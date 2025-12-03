using CityDiscovery.ReviewService.Domain.Entities;

namespace CityDiscovery.ReviewService.Application.Interfaces;

public interface IReviewEventPublisher
{
    Task PublishReviewCreatedAsync(Reviewx review, Guid venueOwnerId, CancellationToken cancellationToken = default);
}
