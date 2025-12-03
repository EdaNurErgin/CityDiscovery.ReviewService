
namespace CityDiscovery.ReviewService.Shared.Events
{
    public class UserDeletedEvent
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}