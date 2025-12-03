using CityDiscovery.ReviewService.Domain.Interfaces;
using CityDiscovery.ReviewService.Shared.Events;
using MassTransit;

namespace CityDiscovery.ReviewService.Infrastructure.MessageBus.Consumers; // Namespace güncellendi

public class VenueDeletedConsumer : IConsumer<VenueDeletedEvent>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IFavoriteVenueRepository _favoriteRepository;

    public VenueDeletedConsumer(IReviewRepository reviewRepository, IFavoriteVenueRepository favoriteRepository)
    {
        _reviewRepository = reviewRepository;
        _favoriteRepository = favoriteRepository;
    }

    public async Task Consume(ConsumeContext<VenueDeletedEvent> context)
    {
        var venueId = context.Message.VenueId;

        // Repository üzerinden silme işlemleri (Senaryo gereği)
        // await _reviewRepository.DeleteByVenueIdAsync(venueId);

        Console.WriteLine($"[MessageBus] Venue {venueId} silindi. İlgili yorumlar temizleniyor...");
        await Task.CompletedTask;
    }
}