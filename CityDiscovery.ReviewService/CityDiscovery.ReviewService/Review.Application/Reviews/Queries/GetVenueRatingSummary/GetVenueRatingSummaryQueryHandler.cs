using CityDiscovery.ReviewService.Application.DTOs;
using CityDiscovery.ReviewService.Domain.Interfaces;
using MediatR;

namespace CityDiscovery.ReviewService.Application.Reviews.Queries.GetVenueRatingSummary;

public sealed class GetVenueRatingSummaryQueryHandler
    : IRequestHandler<GetVenueRatingSummaryQuery, VenueRatingSummaryDto>
{
    private readonly IReviewRepository _reviewRepository;

    public GetVenueRatingSummaryQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<VenueRatingSummaryDto> Handle(GetVenueRatingSummaryQuery request, CancellationToken cancellationToken)
    {
        var reviews = await _reviewRepository.GetVenueReviewsAsync(request.VenueId, cancellationToken);

        var count = reviews.Count;
        var avg = count == 0 ? 0 : reviews.Average(r => r.Rating);

        return new VenueRatingSummaryDto
        {
            VenueId = request.VenueId,
            ReviewCount = count,
            AvgRating = avg
        };
    }
}
