using CityDiscovery.ReviewService.Application.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;

namespace CityDiscovery.ReviewService.Infrastructure.ExternalServices;

public class VenueServiceClient : IVenueServiceClient
{
    private readonly HttpClient _httpClient;

    public VenueServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CheckVenueExistsAsync(Guid venueId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/api/venues/{venueId}/exists", cancellationToken);

        if (!response.IsSuccessStatusCode)
            return false;

        return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken);
    }

    public async Task<Guid> GetVenueOwnerIdAsync(Guid venueId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/api/venues/{venueId}/owner", cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Guid>(cancellationToken: cancellationToken);
    }
}
