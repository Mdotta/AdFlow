using AdFlow.Application.DTOs;
using AdFlow.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdFlow.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IFacebookAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IFacebookAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("facebook-login")]
    public async Task<ActionResult<FacebookLoginResponse>> FacebookLogin([FromBody] FacebookLoginRequest request)
    {
        try
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized login attempt");
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during Facebook login");
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }

    [HttpGet("user/{id}")]
    [Authorize]
    public async Task<ActionResult<FacebookUserDto>> GetUser(Guid id)
    {
        try
        {
            var user = await _authService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("users")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<FacebookUserDto>>> GetAllUsers()
    {
        try
        {
            var users = await _authService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }
}
