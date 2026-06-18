// ============ AuthController ============
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AleCell.API.DTOs;
using AleCell.API.Services.Interfaces;

namespace AleCell.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromForm] RegisterDto registerDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _authService.RegisterAsync(registerDto);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _authService.LoginAsync(loginDto);
        return Ok(result);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();
        var user = await _authService.GetUserByIdAsync(userId);
        return Ok(user);
    }

    [HttpGet("validate")]
    [Authorize]
    public ActionResult ValidateToken() => Ok(new { message = "Token válido", isValid = true });
}
