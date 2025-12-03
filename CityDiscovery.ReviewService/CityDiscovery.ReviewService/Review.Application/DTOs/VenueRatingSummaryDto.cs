namespace CityDiscovery.ReviewService.Application.DTOs;

public class VenueRatingSummaryDto
{
    public Guid VenueId { get; set; }
    public double AvgRating { get; set; }
    public int ReviewCount { get; set; }
}
