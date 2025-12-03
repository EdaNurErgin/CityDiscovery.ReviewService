using MediatR;

namespace CityDiscovery.ReviewService.Application.Reviews.Commands.CreateReview;

public sealed class CreateReviewCommand : IRequest<Guid>
{
    public Guid VenueId { get; init; }
    public Guid UserId { get; init; }   
    public int Rating { get; init; }
    public string Comment { get; init; } = string.Empty;
}
