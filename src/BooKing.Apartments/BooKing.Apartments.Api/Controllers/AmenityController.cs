using BooKing.Apartments.Application.Dtos;
using BooKing.Apartments.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooKing.Apartments.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AmenityController : ControllerBase
{
    private readonly IAmenityService _amenityService;

    public AmenityController(IAmenityService amenityService)
    {
        _amenityService = amenityService;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] NewAmenityDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _amenityService.CreateAsync(dto);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _amenityService.GetAll();
        
        return Ok(result.Value);
    }
}
