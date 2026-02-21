using CityDiscovery.ReviewService.Domain.Interfaces;
using CityDiscovery.ReviewService.ReviewService.Shared.Events.Review;
using CityDiscovery.VenueService.VenuesService.Shared.Common.Events.Review;
using MassTransit;
using MediatR;

namespace CityDiscovery.ReviewService.Application.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public DeleteReviewCommandHandler(IReviewRepository reviewRepository, IPublishEndpoint publishEndpoint)
        {
            _reviewRepository = reviewRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            //  Yorumu veritabanından bul
            var review = await _reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken);

            if (review == null)
                throw new KeyNotFoundException($"Review with ID {request.ReviewId} not found.");

            //  Yetki Kontrolü: Sadece yorum sahibi silebilir
            if (review.UserId != request.UserId)
                throw new UnauthorizedAccessException("You can only delete your own review.");

            //  Yorumu Sil (Bu metodun Repository'de olması şart, aşağıda ekleyeceğiz)
            _reviewRepository.Remove(review);
            await _reviewRepository.SaveChangesAsync(cancellationToken);

            // Ortalama Puanı Tekrar Hesapla (Mekan için)
            var stats = await _reviewRepository.GetVenueRatingStatsAsync(review.VenueId, cancellationToken);

            // 5. Venue Service'e Haber Ver (Mekan Puanını Düşür/Güncelle)
            await _publishEndpoint.Publish(new VenueRatingUpdatedEvent
            {
                VenueId = review.VenueId,
                NewAverageRating = stats.AverageRating,
                TotalReviewCount = stats.ReviewCount
            }, cancellationToken);

            // 6. Admin Notification'a Haber Ver (Bildirimleri Temizle)
            await _publishEndpoint.Publish(new ReviewDeletedEvent
            {
                ReviewId = review.Id,
                VenueId = review.VenueId
            }, cancellationToken);
        }
    }
}