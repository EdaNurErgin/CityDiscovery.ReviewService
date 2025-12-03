namespace CityDiscovery.ReviewService.API.Models.Requests;

public class CreateReviewRequest
{
    public Guid VenueId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}