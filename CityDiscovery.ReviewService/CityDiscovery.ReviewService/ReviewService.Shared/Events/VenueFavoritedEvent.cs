namespace CityDiscovery.ReviewService.Shared.Events;

public class VenueFavoritedEvent
{
    public Guid VenueId { get; set; }
    public Guid UserId { get; set; }
    public DateTime FavoritedAt { get; set; } = DateTime.UtcNow;
}