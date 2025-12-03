using CityDiscovery.ReviewService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CityDiscovery.ReviewService.Review.Infrastructure.Data.Context
{
    public class ReviewDbContextFactory : IDesignTimeDbContextFactory<ReviewDbContext>
    {
        public ReviewDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";

            var basePath = Directory.GetCurrentDirectory();
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connStr = config.GetConnectionString("DefaultConnection")
                         ?? throw new InvalidOperationException("Connection string 'DefaultConnection' bulunamadı.");

            var optionsBuilder = new DbContextOptionsBuilder<ReviewDbContext>();

            optionsBuilder.UseSqlServer(
                connStr,
                sql =>
                {
                    sql.MigrationsAssembly(typeof(ReviewDbContext).Assembly.FullName);
                    sql.UseNetTopologySuite();
                }
            );

            return new ReviewDbContext(optionsBuilder.Options);
        }
    }
}
