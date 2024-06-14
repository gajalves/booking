using BooKing.Identity.Application.Dtos;
using BooKing.Identity.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BooKing.Identity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRegisterService _registerService;
    private readonly IUserLoginService _loginService;

    public UserController(IUserRegisterService registerService, 
                          IUserLoginService loginService)
    {        
        _registerService = registerService;
        _loginService = loginService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _registerService.Register(dto);
        
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _loginService.Login(dto);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
