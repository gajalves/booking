using Booking.Reserve.Application.Dtos;
using Booking.Reserve.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Reserve.Api.Controllers;

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
    public async Task<IActionResult> Resert([FromBody] NewReservationDto dto)
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

        var result = await _reservationService.Reserve(dto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result.Error);
    }
}
