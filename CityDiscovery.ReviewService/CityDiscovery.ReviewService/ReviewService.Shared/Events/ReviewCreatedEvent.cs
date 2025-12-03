namespace CityDiscovery.ReviewService.Shared.Events
{
    public class ReviewCreatedEvent
    {
        public Guid ReviewId { get; set; }
        public Guid VenueId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}