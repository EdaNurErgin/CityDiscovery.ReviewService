using CityDiscovery.ReviewService.Application.DTOs;
using CityDiscovery.ReviewService.Application.Favorites.Commands.AddFavorite;
using CityDiscovery.ReviewService.Application.Favorites.Commands.RemoveFavorite;
using CityDiscovery.ReviewService.Application.Favorites.Queries.GetUserFavorites;
using CityDiscovery.ReviewService.API.Models.Requests; 
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CityDiscovery.ReviewService.API.Controllers;

/// <summary>
/// Favori mekan yönetim endpoint'leri
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Favorites")]
public class FavoritesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FavoritesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Giriş yapmış kullanıcının favori mekanlarını listeler
    /// </summary>
    /// <returns>Favori mekan listesi</returns>
    /// <response code="200">Başarılı - Liste döner</response>
    /// <response code="401">Yetkisiz erişim</response>
    [HttpGet("my")]
    [Authorize]
    [ProducesResponseType(typeof(List<FavoriteVenueDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<FavoriteVenueDto>>> GetMyFavorites()
    {
        if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is Guid userId)
        {
            var result = await _mediator.Send(new GetUserFavoritesQuery { UserId = userId });
            return Ok(result);
        }
        return Unauthorized();
    }

    /// <summary>
    /// Bir kullanıcının favorilerini listeler (Public)
    /// </summary>
    /// <param name="userId">Kullanıcı ID</param>
    [HttpGet("user/{userId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<FavoriteVenueDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<FavoriteVenueDto>>> GetUserFavorites(Guid userId)
    {
        var result = await _mediator.Send(new GetUserFavoritesQuery { UserId = userId });
        return Ok(result);
    }

    /// <summary>
    /// Mekanı favorilere ekler
    /// </summary>
    /// <param name="request">Mekan bilgisi</param>
    /// <remarks>
    /// Giris yapmis kullanici mekani favorilere ekler
    /// </remarks>
    /// <response code="204">Başarılı - Eklendi</response>
    /// <response code="400">Geçersiz istek veya mekan bulunamadı</response>
    /// <response code="401">Yetkisiz erişim</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

    /// <summary>
    /// Mekanı favorilerden çıkarır
    /// </summary>
    /// <param name="request">Mekan bilgisi</param>
    /// <remarks>
    /// Örnek istek:
    /// 
    /// DELETE /api/Favorites
    /// {
    ///   "venueId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    /// }
    /// </remarks>
    [HttpDelete]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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