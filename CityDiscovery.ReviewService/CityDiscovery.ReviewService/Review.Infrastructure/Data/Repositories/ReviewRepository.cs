using CityDiscovery.ReviewService.Domain.Entities;
using CityDiscovery.ReviewService.Domain.Interfaces;
using CityDiscovery.ReviewService.Infrastructure.Data;
using CityDiscovery.ReviewService.Review.Infrastructure.Data.Context;
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
}
