using Api.Data;
using Api.Models.Requests;
using Api.Models.Responses;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
  public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
  {
    var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == request.Username);
    if (user == null || !user.VerifyPassword(request.Password))
    {
      return Unauthorized(new { message = "Invalid credentials" });
    }

    return Ok(new { Token = _jwtService.CreateToken(user) });
  }
}
