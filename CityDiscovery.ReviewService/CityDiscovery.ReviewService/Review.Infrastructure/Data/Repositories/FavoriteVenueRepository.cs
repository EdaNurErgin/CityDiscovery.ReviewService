using CityDiscovery.ReviewService.Domain.Entities;
using CityDiscovery.ReviewService.Domain.Interfaces;
using CityDiscovery.ReviewService.Infrastructure.Data;
using CityDiscovery.ReviewService.Review.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CityDiscovery.ReviewService.Review.Infrastructure.Data.Repositories;

public class FavoriteVenueRepository : IFavoriteVenueRepository
{
    private readonly ReviewDbContext _context;

    public FavoriteVenueRepository(ReviewDbContext context)
    {
        _context = context;
    }

    public Task<bool> ExistsAsync(Guid userId, Guid venueId, CancellationToken cancellationToken = default)
        => _context.FavoriteVenues
            .AnyAsync(f => f.UserId == userId && f.VenueId == venueId, cancellationToken);

    public Task<List<FavoriteVenue>> GetUserFavoritesAsync(Guid userId, CancellationToken cancellationToken = default)
        => _context.FavoriteVenues
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(FavoriteVenue favorite, CancellationToken cancellationToken = default)
    {
        await _context.FavoriteVenues.AddAsync(favorite, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(Guid userId, Guid venueId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.FavoriteVenues
            .FirstOrDefaultAsync(f => f.UserId == userId && f.VenueId == venueId, cancellationToken);

        if (entity is null)
            return;

        _context.FavoriteVenues.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
