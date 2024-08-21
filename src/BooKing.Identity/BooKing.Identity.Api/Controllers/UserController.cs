using BooKing.Identity.Application.Dtos;
using BooKing.Identity.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooKing.Identity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRegisterService _registerService;
    private readonly IUserLoginService _loginService;
    private readonly IUserService _userService;

    public UserController(IUserRegisterService registerService,
                          IUserLoginService loginService,
                          IUserService userService)
    {
        _registerService = registerService;
        _loginService = loginService;
        _userService = userService;
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

    [Authorize]
    [HttpGet("UserInfo/{userId}")]
    public async Task<IActionResult> UserInformation(Guid userId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.GetUserInformation(userId);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
