using CityDiscovery.ReviewService.Application.DTOs;
using MediatR;

namespace CityDiscovery.ReviewService.Application.Reviews.Queries.GetVenueRatingSummary;

public sealed class GetVenueRatingSummaryQuery : IRequest<VenueRatingSummaryDto>
{
    public Guid VenueId { get; init; }
}
