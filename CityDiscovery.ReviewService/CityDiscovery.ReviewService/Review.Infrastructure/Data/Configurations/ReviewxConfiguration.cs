using CityDiscovery.ReviewService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityDiscovery.ReviewService.Infrastructure.Data.Configurations;

public class ReviewxConfiguration : IEntityTypeConfiguration<Reviewx>
{
    public void Configure(EntityTypeBuilder<Reviewx> builder)
    {
        builder.ToTable("Reviews");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Rating)
            .IsRequired();

        builder.Property(x => x.Comment)
            .HasMaxLength(1000);

        builder.HasIndex(x => new { x.VenueId, x.UserId })
            .IsUnique();

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
