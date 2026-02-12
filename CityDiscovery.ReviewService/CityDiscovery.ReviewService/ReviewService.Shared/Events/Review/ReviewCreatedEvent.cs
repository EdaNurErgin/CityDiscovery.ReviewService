namespace CityDiscovery.ReviewService.ReviewService.Shared.Events.Review
{
    public class ReviewCreatedEvent
    {
        public Guid ReviewId { get; set; }
        public Guid VenueId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }

        // Eksik olan alanlar eklendi:
        public string Comment { get; set; }
        public Guid VenueOwnerId { get; set; }

        // Handler'da "CreatedAt" olarak kullanıldığı için isim güncellendi:
        public DateTime CreatedAt { get; set; }
    }
}