namespace CityDiscovery.ReviewService.Domain.Entities;

public class Reviewx
{
    public Guid Id { get; private set; }

    public Guid VenueId { get; private set; }
    public Guid UserId { get; private set; }

    public int Rating { get; private set; }   // 1-5
    public string Comment { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public bool IsDeleted { get; private set; }

    private Reviewx() { } // EF için

    public Reviewx(Guid venueId, Guid userId, int rating, string comment)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");

        Id = Guid.NewGuid();
        VenueId = venueId;
        UserId = userId;
        Rating = rating;
        Comment = comment?.Trim() ?? string.Empty;
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public void Update(int rating, string comment)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentOutOfRangeException(nameof(rating));

        Rating = rating;
        Comment = comment?.Trim() ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
