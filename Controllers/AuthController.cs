using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Services;
using ProductManagementApi.DTOs;

namespace ProductManagementApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDto request)
    {
        if (request.Username == "admin" && request.Password == "password123")
        {
            var token = _tokenService.GenerateJwtToken(request.Username);
            return Ok(new { token });
        }

        return Unauthorized();
    }
}
