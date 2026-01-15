using MediatR;

namespace CityDiscovery.ReviewService.Application.Reviews.Commands.DeleteReview
{
    // Bu komut bir değer döndürmüyor (Unit) veya Task döndürüyor.
    // İstersen silinen ReviewId'yi de döndürebilirsin.
    public record DeleteReviewCommand(Guid ReviewId, Guid UserId) : IRequest;
}