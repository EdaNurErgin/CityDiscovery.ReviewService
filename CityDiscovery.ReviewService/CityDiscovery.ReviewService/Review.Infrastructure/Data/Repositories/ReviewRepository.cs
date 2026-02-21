using CityDiscovery.ReviewService.Domain.Entities;
using CityDiscovery.ReviewService.Domain.Interfaces;
using CityDiscovery.ReviewService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CityDiscovery.ReviewService.Review.Infrastructure.Data.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ReviewDbContext _context;

    public ReviewRepository(ReviewDbContext context)
    {
        _context = context;
    }

    public Task<Reviewx?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _context.Reviews.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public Task<Reviewx?> GetByUserAndVenueAsync(Guid userId, Guid venueId, CancellationToken cancellationToken = default)
        => _context.Reviews.FirstOrDefaultAsync(
            r => r.UserId == userId && r.VenueId == venueId,
            cancellationToken);

    public Task<List<Reviewx>> GetVenueReviewsAsync(Guid venueId, CancellationToken cancellationToken = default)
        => _context.Reviews
            .Where(r => r.VenueId == venueId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Reviewx review, CancellationToken cancellationToken = default)
    {
        await _context.Reviews.AddAsync(review, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Reviewx review, CancellationToken cancellationToken = default)
    {
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync(cancellationToken);
    }


    public async Task UpdateReviewerDetailsAsync(Guid userId, string newUserName, string newAvatarUrl)
    {
        // ExecuteUpdateAsync ile o kullanıcıya ait tüm yorumların yazar bilgilerini
        // veritabanına tek bir sorgu atarak (çok daha performanslı şekilde) güncelliyoruz.
        await _context.Reviews
            .Where(r => r.UserId == userId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(r => r.ReviewerUserName, newUserName)
                .SetProperty(r => r.ReviewerAvatarUrl, newAvatarUrl));
    }
    // İçine ekle:
    public void Remove(Reviewx review)
    {
        _context.Reviews.Remove(review);
    }

    // Ortalama hesaplama metodu (Handler'da kullandık):
    public async Task<(double AverageRating, int ReviewCount)> GetVenueRatingStatsAsync(Guid venueId, CancellationToken cancellationToken)
    {
        var stats = await _context.Reviews
            .Where(r => r.VenueId == venueId)
            .GroupBy(r => r.VenueId)
            .Select(g => new
            {
                Count = g.Count(),
                Avg = g.Average(r => r.Rating)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return (stats?.Avg ?? 0, stats?.Count ?? 0);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    // Venue silindiğinde o mekana ait tüm yorumları siler
    public async Task DeleteReviewsByVenueAsync(Guid venueId, CancellationToken cancellationToken = default)
    {
        // 1. O mekana ait yorumları bul
        var reviews = await _context.Reviews
            .Where(r => r.VenueId == venueId)
            .ToListAsync(cancellationToken);

        if (reviews.Any())
        {
            // 2. Hepsini silme listesine ekle
            _context.Reviews.RemoveRange(reviews);

            // 3. Veritabanına yansıt
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    
    public async Task DeleteReviewsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var reviews = await _context.Reviews
            .Where(r => r.UserId == userId)
            .ToListAsync(cancellationToken);

        if (reviews.Any())
        {
            _context.Reviews.RemoveRange(reviews);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

  
    public async Task<List<Guid>> GetReviewedVenueIdsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews
            .Where(r => r.UserId == userId)
            .Select(r => r.VenueId)
            .Distinct()
            .ToListAsync(cancellationToken);
    }
}
