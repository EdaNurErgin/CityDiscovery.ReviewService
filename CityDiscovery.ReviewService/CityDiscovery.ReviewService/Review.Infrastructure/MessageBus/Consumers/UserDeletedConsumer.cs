//using CityDiscovery.ReviewService.Domain.Interfaces;
//using IdentityService.Shared.MessageBus.Identity;
//using MassTransit;

//namespace CityDiscovery.ReviewService.Infrastructure.MessageBus.Consumers;

//public class UserDeletedConsumer : IConsumer<UserDeletedEvent>
//{
//    private readonly IReviewRepository _reviewRepository;
//    private readonly IFavoriteVenueRepository _favoriteRepository;

//    public UserDeletedConsumer(IReviewRepository reviewRepository, IFavoriteVenueRepository favoriteRepository)
//    {
//        _reviewRepository = reviewRepository;
//        _favoriteRepository = favoriteRepository;
//    }

//    public async Task Consume(ConsumeContext<UserDeletedEvent> context)
//    {
//        var userId = context.Message.UserId;

//        // 1. Kullanıcının yorumlarını sil (veya anonimleştir)
//        // await _reviewRepository.DeleteByUserIdAsync(userId);

//        // 2. Kullanıcının favorilerini sil
//        // await _favoriteRepository.DeleteByUserIdAsync(userId);

//        Console.WriteLine($"[MessageBus] User {context.Message.UserName} ({userId}) silindi. Yorumları ve favorileri temizleniyor...");
//        await Task.CompletedTask;
//    }
//}

using CityDiscovery.ReviewService.Domain.Interfaces;
using IdentityService.Shared.MessageBus.Identity;
using MassTransit;
using Microsoft.Extensions.Logging; // Loglama için eklemeyi unutma

namespace CityDiscovery.ReviewService.Infrastructure.MessageBus.Consumers;

public class UserDeletedConsumer : IConsumer<UserDeletedEvent>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IFavoriteVenueRepository _favoriteRepository;
    // Loglama eklemek iyi bir pratiktir, yoksa console.writeline kullanabilirsin
    // private readonly ILogger<UserDeletedConsumer> _logger; 

    public UserDeletedConsumer(IReviewRepository reviewRepository, IFavoriteVenueRepository favoriteRepository)
    {
        _reviewRepository = reviewRepository;
        _favoriteRepository = favoriteRepository;
    }

    public async Task Consume(ConsumeContext<UserDeletedEvent> context)
    {
        var message = context.Message;

        // 1. Kullanıcının yorumlarını sil
        // Not: IReviewRepository içinde bu metodun tanımlı olduğundan emin ol
        await _reviewRepository.DeleteReviewsByUserIdAsync(message.UserId, context.CancellationToken);

        // 2. Kullanıcının favorilerini sil
        // Not: IFavoriteVenueRepository içinde bu metodun tanımlı olduğundan emin ol
        await _favoriteRepository.DeleteFavoritesByUserIdAsync(message.UserId, context.CancellationToken);

        Console.WriteLine($"[ReviewService] User {message.UserName} ({message.UserId}) silindi ({message.DeletedAtUtc}). Yorumları ve favorileri temizlendi.");
    }
}