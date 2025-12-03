using CityDiscovery.ReviewService.Review.Application.DTOs;
using CityDiscovery.ReviewService.Review.Application.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;

namespace CityDiscovery.ReviewService.Infrastructure.ExternalServices;

public class IdentityServiceClient : IIdentityServiceClient
{
    private readonly HttpClient _httpClient;

    public IdentityServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CheckUserExistsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/api/users/{userId}/exists", cancellationToken);

        if (!response.IsSuccessStatusCode)
            return false;

        return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken);
    }
    public async Task<UserDto?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/api/users/{userId}", cancellationToken);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UserDto>(cancellationToken: cancellationToken);
    }

}
