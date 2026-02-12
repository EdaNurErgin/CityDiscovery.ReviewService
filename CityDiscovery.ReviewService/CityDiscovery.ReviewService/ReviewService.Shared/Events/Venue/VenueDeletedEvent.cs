namespace CityDiscovery.VenueService.VenuesService.Shared.Common.Events.Venue
{
    // Venue Service ile birebir aynı yapıda olmalı.
    // Interface (: IIntegrationEvent) sende yoksa silebilirsin, sorun olmaz.
    // Önemli olan Property isimleri ve tipleridir.

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