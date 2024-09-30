using Microsoft.AspNetCore.Mvc;
using RestApi.Domain;
using RestApi.JWT;
using RestApi.Mappers;
using RestApi.Services;
using RestApi.Mappers.Concretes;

namespace RestApi.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthorizationController : ControllerBase
{
    private readonly AuthorizationService _authorizationService;

    public AuthorizationController(AuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        try
        {
            var user = request.ToDomain();
            var token = await _authorizationService.RegisterUserAsync(user);
            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var token = await _authorizationService.LoginAsync(loginRequest.Email, loginRequest.Password);
            var expiresAt = DateTime.Now.AddDays(JwtConstants.ExpirationDays);
            var authResponse = new AuthResponse(token, expiresAt);

            return Ok(authResponse);
        }
        catch (AuthenticationException exception)
        {
            return StatusCode(400, $"{exception.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

