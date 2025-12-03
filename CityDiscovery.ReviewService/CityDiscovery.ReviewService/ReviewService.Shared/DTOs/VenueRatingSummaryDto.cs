namespace CityDiscovery.ReviewService.Shared.DTOs
{
    public class VenueRatingSummaryDto
    {
        public Guid VenueId { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }
}