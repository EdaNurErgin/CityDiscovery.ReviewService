using Microsoft.EntityFrameworkCore;
using CityDiscovery.ReviewService.Domain.Entities;
using System.Reflection;

namespace CityDiscovery.ReviewService.Infrastructure.Data;

public class ReviewDbContext : DbContext
{
    public ReviewDbContext(DbContextOptions<ReviewDbContext> options) : base(options)
    {
    }

    public DbSet<Reviewx> Reviews => Set<Reviewx>();
    public DbSet<FavoriteVenue> FavoriteVenues => Set<FavoriteVenue>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tüm IEntityTypeConfiguration sınıflarını otomatik uygula
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
