using CityDiscovery.ReviewService.API.Models.Requests; // <--- Bu satırı ekleyin
using CityDiscovery.ReviewService.Application.DTOs;
using CityDiscovery.ReviewService.Application.Reviews.Commands.CreateReview;
using CityDiscovery.ReviewService.Application.Reviews.Commands.DeleteReview;
using CityDiscovery.ReviewService.Application.Reviews.Queries.GetVenueRatingSummary;
using CityDiscovery.ReviewService.Application.Reviews.Queries.GetVenueReviews;
using CityDiscovery.ReviewService.Application.Reviews.Queries.HasUserReviewed;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace CityDiscovery.ReviewService.API.Controllers;

/// <summary>
/// Yorum yönetim endpoint'leri
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Mekana yeni yorum ekler
    /// </summary>
    /// <param name="request">Yorum bilgileri</param>
    /// <returns>Oluşturulan yorumun ID'si</returns>
    /// <response code="201">Başarılı - Yorum oluşturuldu</response>
    /// <response code="400">Geçersiz istek veya kullanıcı daha önce yorum yapmış</response>
    /// <response code="401">Yetkisiz erişim</response>
    /// <remarks>
    /// Örnek istek:
    /// 
    /// POST /api/Reviews
    /// {
    ///   "venueId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///   "rating": 5,
    ///   "comment": "Harika bir atmosferi vardı, kahveleri çok taze."
    /// }
    /// </remarks>

    [HttpPost]
    [Authorize]
    [SwaggerOperation(Summary = "Mekana yorum ekler", Description = "Giriş yapmış kullanıcı bir mekana yorum ve puan ekler.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

    /// <summary>
    /// Bir mekanın yorumlarını listeler
    /// </summary>
    /// <param name="venueId">Mekan ID</param>
    /// <returns>Yorum listesi</returns>
    /// <response code="200">Başarılı - Yorum listesi döner</response>
    [HttpGet("venue/{venueId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<ReviewDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ReviewDto>>> GetVenueReviews(Guid venueId)
    {
        var result = await _mediator.Send(new GetVenueReviewsQuery { VenueId = venueId });
        return Ok(result);
    }

    /// <summary>
    /// Mekanın puan özetini (ortalama puan, toplam yorum) getirir
    /// </summary>
    /// <param name="venueId">Mekan ID</param>
    /// <returns>Puan özeti</returns>
    [HttpGet("venue/{venueId:guid}/summary")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(VenueRatingSummaryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<VenueRatingSummaryDto>> GetVenueRatingSummary(Guid venueId)
    {
        var result = await _mediator.Send(new GetVenueRatingSummaryQuery { VenueId = venueId });
        return Ok(result);
    }

    /// <summary>
    /// Kullanıcının mekana yorum yapıp yapmadığını kontrol eder
    /// </summary>
    /// <param name="userId">Kullanıcı ID</param>
    /// <param name="venueId">Mekan ID</param>
    /// <returns>True/False</returns>
    [HttpGet("user/{userId:guid}/has-reviewed/{venueId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> HasUserReviewed(Guid userId, Guid venueId)
    {
        var result = await _mediator.Send(new HasUserReviewedQuery { UserId = userId, VenueId = venueId });
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id)
    {
        // Token'dan UserId'yi alıyoruz (Güvenlik için)
        // Not: HttpContext.Items yerine User.FindFirst kullanımı daha standarttır ancak
        // eğer JwtConfiguration middleware'iniz Items'a atıyorsa oradan da alabilirsiniz.
        // Aşağıdaki kod User.Claims üzerinden standart okuma yapar.
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Alternatif: Middleware ile Items'a attıysanız:
        // if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is Guid uid) { userId = uid; }

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var userId = Guid.Parse(userIdClaim);

        // DÜZELTME BURADA: Mediator (Tip) yerine _mediator (Field) kullanılmalı
        await _mediator.Send(new DeleteReviewCommand(id, userId));

        return NoContent();
    }
}

