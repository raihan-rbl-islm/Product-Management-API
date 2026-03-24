using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Services;
using ProductManagementApi.DTOs;

namespace ProductManagementApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;

    public UserController(ITokenService tokenService, IUserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        ReadUserDTO? readUserDTO = await _userService.AuthenticateAsync(loginRequestDto);

        if (readUserDTO != null)
        {
            var token = _tokenService.GenerateJwtToken(readUserDTO);
            return Ok(new { token });
        }

        return Unauthorized();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        ReadUserDTO? readUserDTO = await _userService.RegisterAsync(registerRequestDto);

        if (readUserDTO != null)
        {
            var token = _tokenService.GenerateJwtToken(readUserDTO);
            return Ok(new { token });
        }
        return BadRequest("Username already exists.");
    }
}
