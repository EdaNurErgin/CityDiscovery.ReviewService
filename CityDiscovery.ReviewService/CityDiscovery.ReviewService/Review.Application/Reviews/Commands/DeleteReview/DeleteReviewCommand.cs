using MediatR;

namespace CityDiscovery.ReviewService.Application.Reviews.Commands.DeleteReview
{
    
    public record DeleteReviewCommand(Guid ReviewId, Guid UserId) : IRequest;
}