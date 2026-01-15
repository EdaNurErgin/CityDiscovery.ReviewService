namespace CityDiscovery.ReviewService.Shared.Events
{
    public class VenueRatingUpdatedEvent
    {
        public Guid VenueId { get; set; }
        public double NewAverageRating { get; set; }
        public int TotalReviewCount { get; set; }
    }
}