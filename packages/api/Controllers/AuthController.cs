using System.Security.Claims;
using Api.Data;
using Api.Models.Requests;
using Api.Models.Responses;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly AppDbContext _context;
  private readonly IJwtService _jwtService;

  public AuthController(AppDbContext context, IJwtService jwtService)
  {
    _context = context;
    _jwtService = jwtService;
  }

  [HttpPost("login")]
  [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
  public ActionResult<LoginResponse> Login(LoginRequest request)
  {
    var user = _context.Users.SingleOrDefault(u => u.Username == request.Username);
    if (user == null || !user.VerifyPassword(request.Password))
    {
      return Unauthorized(new { message = "Invalid credentials" });
    }

    // Generate JWT token
    var claims = new[]
    {
      new Claim(ClaimTypes.Name, user.Username),
      new Claim(ClaimTypes.Role, user.Role),
    };

    var token = _jwtService.CreateToken(claims);
    return Ok(new { Token = token });
  }
}
