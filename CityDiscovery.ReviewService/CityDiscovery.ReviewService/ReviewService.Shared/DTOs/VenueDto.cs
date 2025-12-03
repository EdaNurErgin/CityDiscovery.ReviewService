namespace CityDiscovery.ReviewService.Review.Application.DTOs
{
    // VenueService'den gelen veriyi karşılamak için kullanılan model
    public class VenueDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid OwnerUserId { get; set; } // VenueService'deki mülk sahibi ID'si
        public string? ProfilePictureUrl { get; set; }
    }
}