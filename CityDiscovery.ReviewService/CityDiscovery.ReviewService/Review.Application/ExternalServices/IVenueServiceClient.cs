namespace CityDiscovery.ReviewService.Application.Interfaces;

using CityDiscovery.ReviewService.Review.Application.DTOs;
using CityDiscovery.ReviewService.Shared.DTOs; // VenueDto için namespace
public interface IVenueServiceClient
{
    Task<bool> CheckVenueExistsAsync(Guid venueId, CancellationToken cancellationToken = default);
    Task<Guid> GetVenueOwnerIdAsync(Guid venueId, CancellationToken cancellationToken = default);
    Task<VenueDto?> GetVenueAsync(Guid venueId, CancellationToken cancellationToken = default);
}
