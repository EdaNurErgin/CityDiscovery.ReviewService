using System.ComponentModel.DataAnnotations;

namespace CityDiscovery.ReviewService.API.Models.Requests;

/// <summary>
/// Favorilere ekleme isteği
/// </summary>
public class AddFavoriteRequest
{
    /// <summary>
    /// Favorilere eklenecek mekan ID
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [Required]
    public Guid VenueId { get; set; }
}