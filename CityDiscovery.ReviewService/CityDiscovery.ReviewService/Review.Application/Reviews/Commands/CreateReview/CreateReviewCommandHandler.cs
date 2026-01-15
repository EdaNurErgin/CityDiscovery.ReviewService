using CityDiscovery.ReviewService.Application.Interfaces;
using CityDiscovery.ReviewService.Domain.Entities;
using CityDiscovery.ReviewService.Domain.Interfaces;
using CityDiscovery.ReviewService.Review.Application.Interfaces;
using CityDiscovery.ReviewService.Shared.Events;
using MassTransit;
using MediatR;

namespace CityDiscovery.ReviewService.Application.Reviews.Commands.CreateReview;

public sealed class CreateReviewCommandHandler
    : IRequestHandler<CreateReviewCommand, Guid>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IIdentityServiceClient _identityClient;
    private readonly IVenueServiceClient _venueClient;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateReviewCommandHandler(
        IReviewRepository reviewRepository,
        IIdentityServiceClient identityClient,
        IVenueServiceClient venueClient,
        IPublishEndpoint publishEndpoint)
    {
        _reviewRepository = reviewRepository;
        _identityClient = identityClient;
        _venueClient = venueClient;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
            throw new InvalidOperationException("UserId is required.");


        var userDto = await _identityClient.GetUserAsync(request.UserId, cancellationToken);
        if (userDto == null)
            throw new InvalidOperationException("User not found.");
     

        var venue = await _venueClient.GetVenueAsync(request.VenueId, cancellationToken);

        if (venue == null)
            throw new InvalidOperationException("Venue not found.");

        var ownerId = venue.OwnerUserId;

        if (ownerId == request.UserId)
            throw new InvalidOperationException("Owner cannot review own venue.");

        var existing = await _reviewRepository
            .GetByUserAndVenueAsync(request.UserId, request.VenueId, cancellationToken);

        if (existing is not null)
            throw new InvalidOperationException("User has already reviewed this venue.");


        // YENİSİ (Entity'deki yeni constructor'a uygun olarak):
        var review = new Reviewx(
            request.VenueId,
            request.UserId,
            request.Rating,
            request.Comment,
            userDto.UserName,                // Identity Service'den gelen kullanıcı adı
            userDto.ProfilePictureUrl        // Identity Service'den gelen profil fotosu (DTO'daki isim UserDto dosyasına göre değişebilir, genelde AvatarUrl veya ProfilePictureUrl'dir)
        );


        await _reviewRepository.AddAsync(review, cancellationToken);

        await _publishEndpoint.Publish(new ReviewCreatedEvent
        {
            ReviewId = review.Id,
            VenueId = review.VenueId,
            UserId = review.UserId,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = DateTime.UtcNow,
            VenueOwnerId = ownerId
        }, cancellationToken);

        return review.Id;
    }
}