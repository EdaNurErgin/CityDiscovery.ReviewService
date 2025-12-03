using CityDiscovery.ReviewService.Application.Interfaces;
using CityDiscovery.ReviewService.Domain.Entities;
using CityDiscovery.ReviewService.Domain.Interfaces;
using CityDiscovery.ReviewService.Review.Application.Interfaces;
using MediatR;


namespace CityDiscovery.ReviewService.Application.Reviews.Commands.CreateReview;

public sealed class CreateReviewCommandHandler
    : IRequestHandler<CreateReviewCommand, Guid>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IIdentityServiceClient _identityClient;
    private readonly IVenueServiceClient _venueClient;

    public CreateReviewCommandHandler(
        IReviewRepository reviewRepository,
        IIdentityServiceClient identityClient,
        IVenueServiceClient venueClient)
    {
        _reviewRepository = reviewRepository;
        _identityClient = identityClient;
        _venueClient = venueClient;
    }

    public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
            throw new InvalidOperationException("UserId is required.");

        var userExists = await _identityClient.CheckUserExistsAsync(request.UserId, cancellationToken);
        if (!userExists)
            throw new InvalidOperationException("User not found.");

        var venueExists = await _venueClient.CheckVenueExistsAsync(request.VenueId, cancellationToken);
        if (!venueExists)
            throw new InvalidOperationException("Venue not found.");

        
        var ownerId = await _venueClient.GetVenueOwnerIdAsync(request.VenueId, cancellationToken);
        if (ownerId == request.UserId)
            throw new InvalidOperationException("Owner cannot review own venue.");

        var existing = await _reviewRepository
            .GetByUserAndVenueAsync(request.UserId, request.VenueId, cancellationToken);

        if (existing is not null)
            throw new InvalidOperationException("User has already reviewed this venue.");

        var review = new Reviewx(request.VenueId, request.UserId, request.Rating, request.Comment);

        
        await _reviewRepository.AddAsync(review, cancellationToken);

        

        return review.Id;
    }
}
