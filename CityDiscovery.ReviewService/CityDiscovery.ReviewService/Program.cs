using CityDiscovery.ReviewService.Application.DependencyInjection;
using CityDiscovery.ReviewService.Application.Interfaces;
using CityDiscovery.ReviewService.Infrastructure.Data;
using CityDiscovery.ReviewService.Infrastructure.DependencyInjection;
using CityDiscovery.ReviewService.Infrastructure.ExternalServices;
using CityDiscovery.ReviewService.Review.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
namespace CityDiscovery.ReviewService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddReviewInfrastructure(builder.Configuration);
            builder.Services.AddReviewApplication(builder.Configuration);

            // --- HTTP CLIENT KAYITLARI ---
            builder.Services.AddHttpClient<IVenueServiceClient, VenueServiceClient>(client =>
            {
                var url = builder.Configuration["ServiceUrls:VenueService"];
                if (string.IsNullOrEmpty(url))
                {
                    throw new Exception("ServiceUrls:VenueService yapılandırması appsettings.json içinde bulunamadı!");
                }
                client.BaseAddress = new Uri(url);
                Console.WriteLine($"[Review Service] VenueServiceClient Yapılandırıldı: {client.BaseAddress}");
            });

            builder.Services.AddHttpClient<IIdentityServiceClient, IdentityServiceClient>(client =>
            {
                var url = builder.Configuration["ServiceUrls:IdentityService"];
                if (string.IsNullOrEmpty(url))
                {
                    throw new Exception("ServiceUrls:IdentityService yapılandırması appsettings.json içinde bulunamadı!");
                }
                client.BaseAddress = new Uri(url);
                Console.WriteLine($"[Review Service] IdentityServiceClient Yapılandırıldı: {client.BaseAddress}");
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CityDiscovery Review Service API",
                    Version = "v1",
                    Description = "Mekan yorumlar? ve favori i?lemleri için Review Service API dokümantasyonu.",
                    Contact = new OpenApiContact
                    {
                        Name = "CityDiscovery Team"
                    }
                });

                // XML Dokümantasyonunu Dahil Et
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                // Annotation'lar? aktif et (SwaggerOperation vb. için)
                c.EnableAnnotations();

                // JWT Güvenlik Tan?m?
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Sadece token'? yap??t?r?n."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });


            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            // Health Checks
            builder.Services.AddHealthChecks();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dbContext = services.GetRequiredService<ReviewDbContext>();
                    dbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the social database.");
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication(); 
            app.UseAuthorization();


            // Health Check Endpoint
            app.MapHealthChecks("/health");

            app.MapControllers();

            app.Run();
        }
    }
}

