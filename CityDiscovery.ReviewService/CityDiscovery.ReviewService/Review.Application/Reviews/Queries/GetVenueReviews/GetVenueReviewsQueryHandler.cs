using CityDiscovery.ReviewService.Application.DTOs;
using CityDiscovery.ReviewService.Domain.Interfaces;
using MediatR;

namespace CityDiscovery.ReviewService.Application.Reviews.Queries.GetVenueReviews;

public sealed class GetVenueReviewsQueryHandler
    : IRequestHandler<GetVenueReviewsQuery, List<ReviewDto>>
{
    private readonly IReviewRepository _reviewRepository;

    public GetVenueReviewsQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<List<ReviewDto>> Handle(GetVenueReviewsQuery request, CancellationToken cancellationToken)
    {
        var reviews = await _reviewRepository.GetVenueReviewsAsync(request.VenueId, cancellationToken);

        
        return reviews.Select(r => new ReviewDto
        {
            Id = r.Id,
            VenueId = r.VenueId,
            UserId = r.UserId,
            ReviewerUserName = r.ReviewerUserName,
            ReviewerAvatarUrl = r.ReviewerAvatarUrl,
            Rating = r.Rating,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt
        }).ToList();
    }
}
