using CityDiscovery.ReviewService.Review.Application.DTOs;
namespace CityDiscovery.ReviewService.Review.Application.Interfaces
{
    public interface IIdentityServiceClient
    {
        // Kullanıcının var olup olmadığını kontrol eder
        Task<bool> CheckUserExistsAsync(Guid userId, CancellationToken cancellationToken = default);

        // Kullanıcı detaylarını getirir (ReviewDto doldururken lazım olacak)
        Task<UserDto?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}