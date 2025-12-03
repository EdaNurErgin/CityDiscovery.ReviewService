using CityDiscovery.ReviewService.Application.DTOs;
using CityDiscovery.ReviewService.Application.Reviews.Commands.CreateReview;
using CityDiscovery.ReviewService.Application.Reviews.Queries.GetVenueRatingSummary;
using CityDiscovery.ReviewService.Application.Reviews.Queries.GetVenueReviews;
using CityDiscovery.ReviewService.API.Models.Requests; // <--- Bu satırı ekleyin
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscovery.ReviewService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST /api/reviews
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateReviewRequest request)
    {
        // Token'dan UserId'yi al (JwtConfiguration sayesinde HttpContext.Items'da var)
        if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is Guid userId)
        {
            var command = new CreateReviewCommand
            {
                VenueId = request.VenueId,
                UserId = userId,
                Rating = request.Rating,
                Comment = request.Comment
            };

            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetVenueReviews), new { venueId = command.VenueId }, new { id });
        }

        return Unauthorized("User ID could not be retrieved from token.");
    }

    // GET /api/reviews/venue/{venueId}
    [HttpGet("venue/{venueId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<ReviewDto>>> GetVenueReviews(Guid venueId)
    {
        var result = await _mediator.Send(new GetVenueReviewsQuery { VenueId = venueId });
        return Ok(result);
    }

    // GET /api/reviews/venue/{venueId}/summary
    [HttpGet("venue/{venueId:guid}/summary")]
    [AllowAnonymous]
    public async Task<ActionResult<VenueRatingSummaryDto>> GetVenueRatingSummary(Guid venueId)
    {
        var result = await _mediator.Send(new GetVenueRatingSummaryQuery { VenueId = venueId });
        return Ok(result);
    }
}

