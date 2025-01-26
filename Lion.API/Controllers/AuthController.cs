using Lion.Application.Common.Interfaces;
using Lion.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lion.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _identityService.CreateUserAsync(registerDto);
        if (result.Success)
        {
            return Ok(new { UserId = result.UserId });
        }

        return BadRequest("Registration failed");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await _identityService.LoginAsync(loginDto);
        if (result.Success)
        {
            return Ok(new { Token = result.Token });
        }

        return BadRequest("Invalid credentials");
    }
}
