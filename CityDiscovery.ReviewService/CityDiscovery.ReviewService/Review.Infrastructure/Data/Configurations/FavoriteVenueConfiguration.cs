using CityDiscovery.ReviewService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityDiscovery.ReviewService.Infrastructure.Data.Configurations;

public class FavoriteVenueConfiguration : IEntityTypeConfiguration<FavoriteVenue>
{
    public void Configure(EntityTypeBuilder<FavoriteVenue> builder)
    {
        builder.ToTable("FavoriteVenues");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.UserId, x.VenueId })
            .IsUnique();
    }
}
