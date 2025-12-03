namespace CityDiscovery.ReviewService.Shared.Events
{
    public class ReviewDeletedEvent
    {
        public Guid ReviewId { get; set; }
        public Guid VenueId { get; set; }
    }
}