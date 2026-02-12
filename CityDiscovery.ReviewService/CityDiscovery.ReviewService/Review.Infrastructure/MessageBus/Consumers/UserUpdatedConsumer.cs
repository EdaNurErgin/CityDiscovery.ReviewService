using CityDiscovery.ReviewService.Application.Interfaces;
using CityDiscovery.ReviewService.Domain.Interfaces;
using CityDiscovery.ReviewService.ReviewService.Shared.Events.Identity;
using MassTransit;

namespace CityDiscovery.ReviewService.Infrastructure.MessageBus.Consumers
{
    // Not: UserUpdatedEvent sınıfının kopyası Review projesinde de olmalı veya Shared library kullanılmalı.
    public class UserUpdatedConsumer : IConsumer<UserUpdatedEvent>
    {
        private readonly IReviewRepository _reviewRepository;

        public UserUpdatedConsumer(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
        {
            // Repository'e "Bu kullanıcının tüm yorumlarını güncelle" metodu eklenmeli
            await _reviewRepository.UpdateReviewerDetailsAsync(
                context.Message.UserId,
                context.Message.NewUserName,
                context.Message.NewAvatarUrl
            );
        }
    }
}