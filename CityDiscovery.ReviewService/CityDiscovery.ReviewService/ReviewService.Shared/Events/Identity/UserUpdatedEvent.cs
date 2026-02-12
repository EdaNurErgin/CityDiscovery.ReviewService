namespace CityDiscovery.ReviewService.ReviewService.Shared.Events.Identity
{
    public class UserUpdatedEvent
    {
        public Guid UserId { get; set; }
        public string NewUserName { get; set; }
        public string NewAvatarUrl { get; set; }
    }
}