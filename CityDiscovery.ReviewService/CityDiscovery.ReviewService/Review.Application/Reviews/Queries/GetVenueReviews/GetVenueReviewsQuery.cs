using CityDiscovery.ReviewService.Application.DTOs;
using MediatR;

namespace CityDiscovery.ReviewService.Application.Reviews.Queries.GetVenueReviews;

public sealed class GetVenueReviewsQuery : IRequest<List<ReviewDto>>
{
    public Guid VenueId { get; init; }
}
