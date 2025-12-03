using System.ComponentModel.DataAnnotations;

namespace CityDiscovery.ReviewService.API.Models.Requests;

/// <summary>
/// Yeni yorum oluşturma isteği
/// </summary>
public class CreateReviewRequest
{
    /// <summary>
    /// Yorum yapılacak mekanın ID'si
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [Required]
    public Guid VenueId { get; set; }
    /// <summary>
    /// Puan (1-5 arası)
    /// </summary>
    /// <example>5</example>
    [Range(1, 5, ErrorMessage = "Puan 1 ile 5 arasında olmalıdır.")]
    public int Rating { get; set; }
    /// <summary>
    /// Yorum içeriği
    /// </summary>
    /// <example>Harika bir atmosferi vardı, kahveleri çok taze.</example>
    [MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;
}