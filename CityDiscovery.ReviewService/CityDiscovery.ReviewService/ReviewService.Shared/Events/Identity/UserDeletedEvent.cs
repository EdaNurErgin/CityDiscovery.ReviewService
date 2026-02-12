namespace CityDiscovery.ReviewService.ReviewService.Shared.Events.Identity
{
    public class UserDeletedEvent
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}