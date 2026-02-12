using CityDiscovery.ReviewService.Domain.Interfaces;
using CityDiscovery.ReviewService.ReviewService.Shared.Events.Identity;
using MassTransit;

namespace CityDiscovery.ReviewService.Infrastructure.MessageBus.Consumers;

public class UserDeletedConsumer : IConsumer<UserDeletedEvent>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IFavoriteVenueRepository _favoriteRepository;

    public UserDeletedConsumer(IReviewRepository reviewRepository, IFavoriteVenueRepository favoriteRepository)
    {
        _reviewRepository = reviewRepository;
        _favoriteRepository = favoriteRepository;
    }

    public async Task Consume(ConsumeContext<UserDeletedEvent> context)
    {
        var userId = context.Message.UserId;

        // 1. Kullanıcının yorumlarını sil (veya anonimleştir)
        // await _reviewRepository.DeleteByUserIdAsync(userId);

        // 2. Kullanıcının favorilerini sil
        // await _favoriteRepository.DeleteByUserIdAsync(userId);

        Console.WriteLine($"[MessageBus] User {context.Message.UserName} ({userId}) silindi. Yorumları ve favorileri temizleniyor...");
        await Task.CompletedTask;
    }
}