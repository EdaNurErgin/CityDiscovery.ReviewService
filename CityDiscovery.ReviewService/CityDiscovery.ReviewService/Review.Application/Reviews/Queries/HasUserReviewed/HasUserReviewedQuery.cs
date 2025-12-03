using MediatR;

namespace CityDiscovery.ReviewService.Application.Reviews.Queries.HasUserReviewed;

public class HasUserReviewedQuery : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid VenueId { get; set; }
}