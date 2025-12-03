namespace CityDiscovery.ReviewService.Shared.DTOs
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public Guid VenueId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } // UI'da göstermek için yararlı olabilir
        public string? UserProfileImageUrl { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}