namespace CityDiscovery.ReviewService.Application.DTOs;

public class ReviewDto
{
    public Guid Id { get; set; }
    public Guid VenueId { get; set; }
    public Guid UserId { get; set; }
    public string? ReviewerUserName { get; set; }
    public string? ReviewerAvatarUrl { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
