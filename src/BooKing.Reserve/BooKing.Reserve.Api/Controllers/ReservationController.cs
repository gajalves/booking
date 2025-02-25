using BooKing.Reserve.Application.Dtos;
using BooKing.Reserve.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooKing.Reserve.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost("Reserve")]
    public async Task<IActionResult> Reserve([FromBody] NewReservationDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _reservationService.Reserve(dto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result.Error);
    }

    [HttpPost("Confirm")]
    public async Task<IActionResult> Confirm([FromBody] Guid reservationId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _reservationService.ConfirmReserve(reservationId);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result.Error);
    }

    [HttpPost("Cancel")]
    public async Task<IActionResult> Cancel([FromBody] Guid reservationId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _reservationService.CancelReserve(reservationId);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result.Error);
    }

    [HttpGet("UserReservations/{userId}")]
    public async Task<IActionResult> UserReservations(Guid userId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _reservationService.GetAllReservationsByUserId(userId);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result.Error);
    }

    [HttpGet("{reservationId}")]
    public async Task<IActionResult> GetReservation(Guid reservationId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _reservationService.GetReservation(reservationId);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result.Error);
    }

    [HttpGet("ReservationEvents/{reservationId}")]
    public async Task<IActionResult> ReservationEvents(Guid reservationId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _reservationService.GetReservationEvents(reservationId);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result.Error);
    }

    [HttpGet("CountUserReservations")]
    public async Task<IActionResult> CountUserReservations()
    {
        var result = await _reservationService.CountUserReservations();

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result.Error);
    }
}
