using CityDiscovery.ReviewService.Domain.Interfaces;
using MediatR;

namespace CityDiscovery.ReviewService.Application.Reviews.Queries.HasUserReviewed;

public class HasUserReviewedQueryHandler : IRequestHandler<HasUserReviewedQuery, bool>
{
    private readonly IReviewRepository _reviewRepository;

    public HasUserReviewedQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<bool> Handle(HasUserReviewedQuery request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByUserAndVenueAsync(request.UserId, request.VenueId, cancellationToken);
        return review != null;
    }
}