using CityDiscovery.AdminNotificationService.Shared.Common.Events.AdminNotification;
using CityDiscovery.ReviewService.Infrastructure.Data; 
using MassTransit;


namespace CityDiscovery.ReviewService.Consumers
{
    public class ContentRemovedConsumer : IConsumer<ContentRemovedEvent>
    {
        private readonly ReviewDbContext _context; // Review veritabanı

        public ContentRemovedConsumer(ReviewDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<ContentRemovedEvent> context)
        {
            var message = context.Message;

            // Sadece Yorum (Review) gelirse çalış:
            if (message.ContentType == "Review" || message.ContentType == "VenueReview")
            {
                // 'Reviews' tablosundan sil
                var review = await _context.Reviews.FindAsync(message.ContentId);

                if (review != null)
                {
                    _context.Reviews.Remove(review);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"[ReviewService] Yorum silindi: {message.ContentId}");
                }
            }
        }
    }
}