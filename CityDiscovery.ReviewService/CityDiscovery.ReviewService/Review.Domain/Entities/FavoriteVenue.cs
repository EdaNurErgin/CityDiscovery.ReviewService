namespace CityDiscovery.ReviewService.Domain.Entities;

public class FavoriteVenue
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }
    public Guid VenueId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private FavoriteVenue() { } // EF için

    public FavoriteVenue(Guid userId, Guid venueId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        VenueId = venueId;
        CreatedAt = DateTime.UtcNow;
    }
}
