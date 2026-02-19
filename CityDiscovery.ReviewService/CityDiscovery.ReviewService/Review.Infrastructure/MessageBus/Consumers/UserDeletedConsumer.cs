using CityDiscovery.ReviewService.Domain.Interfaces;
using IdentityService.Shared.MessageBus.Identity;
using MassTransit;
using CityDiscovery.VenueService.VenuesService.Shared.Common.Events.Review; 

namespace CityDiscovery.ReviewService.Infrastructure.MessageBus.Consumers;

public class UserDeletedConsumer : IConsumer<UserDeletedEvent>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IFavoriteVenueRepository _favoriteRepository;
    private readonly IPublishEndpoint _publishEndpoint; 

  
    public UserDeletedConsumer(
        IReviewRepository reviewRepository,
        IFavoriteVenueRepository favoriteRepository,
        IPublishEndpoint publishEndpoint) 
    {
        _reviewRepository = reviewRepository;
        _favoriteRepository = favoriteRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<UserDeletedEvent> context)
    {
        var message = context.Message;

        // ADIM 1: Yorumları silmeden HEMEN ÖNCE kullanıcının yorum yaptığı mekanların ID'lerini bul
        var affectedVenueIds = await _reviewRepository.GetReviewedVenueIdsByUserIdAsync(message.UserId, context.CancellationToken);

        // 1. Kullanıcının yorumlarını sil
        // Not: IReviewRepository içinde bu metodun tanımlı olduğundan emin ol
        await _reviewRepository.DeleteReviewsByUserIdAsync(message.UserId, context.CancellationToken);

        // 2. Kullanıcının favorilerini sil
        // Not: IFavoriteVenueRepository içinde bu metodun tanımlı olduğundan emin ol
        await _favoriteRepository.DeleteFavoritesByUserIdAsync(message.UserId, context.CancellationToken);

        // YENİ ADIM 2: Yorumlar silindikten sonra, etkilenen mekanların puanlarını hesapla ve bildir
        if (affectedVenueIds != null && affectedVenueIds.Any())
        {
            foreach (var venueId in affectedVenueIds)
            {
                // Güncel ortalamayı ve yorum sayısını al
                var stats = await _reviewRepository.GetVenueRatingStatsAsync(venueId, context.CancellationToken);

                // VenueService'in dinleyip kendi DB'sini güncelleyebilmesi için Event fırlat
                await _publishEndpoint.Publish(new VenueRatingUpdatedEvent
                {
                    VenueId = venueId,
                    NewAverageRating = stats.AverageRating,
                    TotalReviewCount = stats.ReviewCount
                }, context.CancellationToken);
            }
        }

        Console.WriteLine($"[ReviewService] User {message.UserName} ({message.UserId}) silindi ({message.DeletedAtUtc}). Yorumları ve favorileri temizlendi. {(affectedVenueIds?.Count ?? 0)} mekanın puanı güncellendi.");
    }
}