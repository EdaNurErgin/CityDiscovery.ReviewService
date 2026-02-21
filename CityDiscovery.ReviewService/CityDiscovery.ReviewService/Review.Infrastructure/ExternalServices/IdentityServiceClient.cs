using CityDiscovery.ReviewService.Review.Application.DTOs;
using CityDiscovery.ReviewService.Review.Application.Interfaces;


namespace CityDiscovery.ReviewService.Infrastructure.ExternalServices;

public class IdentityServiceClient : IIdentityServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor; // Eklendi

    // Constructor'a IHttpContextAccessor eklendi
    public IdentityServiceClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> CheckUserExistsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // Bu endpoint Identity Service'de [AllowAnonymous] olduğu için token'a gerek yoktu ama eklemekte zarar gelmez.
        var response = await _httpClient.GetAsync($"/api/users/{userId}/exists", cancellationToken);
        if (!response.IsSuccessStatusCode) return false;
        return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken);
    }

    public async Task<UserDto?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // 1. Mevcut isteğin Header'ından Token'ı al
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

        // 2. Eğer token varsa, giden isteğin header'ına ekle
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);
        }

        // 3. İsteği gönder 
        var response = await _httpClient.GetAsync($"/api/users/{userId}", cancellationToken);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UserDto>(cancellationToken: cancellationToken);
    }
}