using CityDiscovery.ReviewService.Application.DTOs;
using CityDiscovery.ReviewService.Application.Favorites.Commands.AddFavorite;
using CityDiscovery.ReviewService.Application.Favorites.Commands.RemoveFavorite;
using CityDiscovery.ReviewService.Application.Favorites.Queries.GetUserFavorites;
using CityDiscovery.ReviewService.API.Models.Requests; 
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscovery.ReviewService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FavoritesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET /api/favorites/my
    // Kullanıcının kendi favorilerini getirmesi için yeni bir endpoint
    [HttpGet("my")]
    [Authorize]
    public async Task<ActionResult<List<FavoriteVenueDto>>> GetMyFavorites()
    {
        if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is Guid userId)
        {
            var result = await _mediator.Send(new GetUserFavoritesQuery { UserId = userId });
            return Ok(result);
        }
        return Unauthorized();
    }

    // GET /api/favorites/user/{userId}
    // Başkasının favorilerini görmek için (İsteğe bağlı, public yapılabilir)
    [HttpGet("user/{userId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<FavoriteVenueDto>>> GetUserFavorites(Guid userId)
    {
        var result = await _mediator.Send(new GetUserFavoritesQuery { UserId = userId });
        return Ok(result);
    }

    // POST /api/favorites
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteRequest request)
    {
        if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is Guid userId)
        {
            var command = new AddFavoriteCommand
            {
                UserId = userId,
                VenueId = request.VenueId
            };

            await _mediator.Send(command);
            return NoContent();
        }

        return Unauthorized();
    }

    // DELETE /api/favorites
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> RemoveFavorite([FromBody] RemoveFavoriteRequest request)
    {
        if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is Guid userId)
        {
            var command = new RemoveFavoriteCommand
            {
                UserId = userId,
                VenueId = request.VenueId
            };

            await _mediator.Send(command);
            return NoContent();
        }

        return Unauthorized();
    }
}