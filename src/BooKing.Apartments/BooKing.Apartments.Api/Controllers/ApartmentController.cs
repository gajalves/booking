using BooKing.Apartments.Application.Dtos;
using BooKing.Apartments.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooKing.Apartments.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ApartmentController : ControllerBase
{
    private readonly IApartmentService _apartmentService;

    public ApartmentController(IApartmentService apartmentService)
    {
        _apartmentService = apartmentService;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] NewApartmentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _apartmentService.CreateApartmentAsync(dto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result.Error);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetPaginated([FromQuery] int pageIndex, int pageSize)
    {
        var result = await _apartmentService.GetPaginatedApartmentsAsync(pageIndex, pageSize);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _apartmentService.GetApartmentByIdAsync(id);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return NotFound(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _apartmentService.DeleteApartmentAsync(id);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApartment(Guid id, [FromBody] UpdateApartmentDto dto)
    {
        var result = await _apartmentService.UpdateApartmentAsync(id, dto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result.Error);
    }

    [HttpPost("GetApartmentsByGuids")]
    public async Task<IActionResult> Create([FromBody] List<Guid> apartmentGuids)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _apartmentService.GetApartmentsByGuids(apartmentGuids);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result.Error);
    }

    [HttpGet("UserApartments/{userId}")]
    public async Task<IActionResult> UserApartments(Guid userId)
    {
        var result = await _apartmentService.GetApartmentsByUserId(userId);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return NotFound(result.Error);
    }

    [HttpPatch("{apartmentId}/IsActive")]
    public async Task<IActionResult> IsActive(Guid apartmentId, [FromBody] PatchApartmentIsActiveDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _apartmentService.PatchApartmentIsActive(apartmentId, dto.IsActive);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result.Error);
    }

    [HttpGet("CountUserApartmentsCreated")]
    public async Task<IActionResult> CountUserApartmentsCreated()
    {
        var result = await _apartmentService.CountUserApartmentsCreated();

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return NotFound(result.Error);
    }
}
