using Api.Models.Requests;
using Api.Models.Responses;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IJwtService _jwtService;

  public AuthController(IJwtService jwtService)
  {
    _jwtService = jwtService;
  }

  [HttpPost("login")]
  [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
  {
    try
    {
      var response = await _jwtService.Login(request.Username, request.Password);
      return Ok(response);
    }
    catch (UnauthorizedAccessException)
    {
      return Unauthorized(new { message = "Invalid credentials" });
    }
  }
}
