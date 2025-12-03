namespace CityDiscovery.ReviewService.Application.Interfaces;

public interface IVenueServiceClient
{
    Task<bool> CheckVenueExistsAsync(Guid venueId, CancellationToken cancellationToken = default);
    Task<Guid> GetVenueOwnerIdAsync(Guid venueId, CancellationToken cancellationToken = default);
}
