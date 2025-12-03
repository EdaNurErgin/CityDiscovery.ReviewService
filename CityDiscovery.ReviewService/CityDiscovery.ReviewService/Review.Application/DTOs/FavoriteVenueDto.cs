namespace CityDiscovery.ReviewService.Application.DTOs;

public class FavoriteVenueDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid VenueId { get; set; }
    public DateTime CreatedAt { get; set; }
}
