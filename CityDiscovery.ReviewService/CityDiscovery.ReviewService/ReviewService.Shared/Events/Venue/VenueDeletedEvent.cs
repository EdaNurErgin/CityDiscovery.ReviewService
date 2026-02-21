namespace CityDiscovery.VenueService.VenuesService.Shared.Common.Events.Venue
{
   

    public class VenueDeletedEvent
    {
        // Gönderen tarafta bunlar "get" idi, burada "set" de ekledik ki veri içine yazılabilsin.
        public Guid Id { get; set; }
        public DateTime OccurredOn { get; set; }

        public Guid VenueId { get; set; }
        public string VenueName { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}