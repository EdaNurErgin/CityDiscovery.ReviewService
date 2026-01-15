namespace CityDiscovery.ReviewService.Domain.Entities;

public class Reviewx
{
    public Guid Id { get; private set; }
    public Guid VenueId { get; private set; }
    public Guid UserId { get; private set; }
    public int Rating { get; private set; }
    public string Comment { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    // Eklediğiniz alanlar
    public string ReviewerUserName { get; private set; } // private set yaptık (dışarıdan değişmesin)
    public string ReviewerAvatarUrl { get; private set; }

    private Reviewx() { }

    // Constructor'ı güncelledik: userName ve avatarUrl alıyor
    public Reviewx(Guid venueId, Guid userId, int rating, string comment, string userName, string avatarUrl)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");

        Id = Guid.NewGuid();
        VenueId = venueId;
        UserId = userId;
        Rating = rating;
        Comment = comment?.Trim() ?? string.Empty;

        // Yeni alanları burada dolduruyoruz. Boş gelirse default değer atıyoruz.
        ReviewerUserName = userName ?? "Anonymous";
        ReviewerAvatarUrl = avatarUrl ?? "";

        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public void Update(int rating, string comment)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentOutOfRangeException(nameof(rating));

        Rating = rating;
        Comment = comment?.Trim() ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
}