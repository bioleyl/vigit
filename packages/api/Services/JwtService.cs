using System.Security.Claims;
using System.Text;
using Api.Entities;
using Api.Models.Responses;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Api.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services;

public class JwtService : IJwtService
{
  private readonly JwtSettings _jwt;
  private readonly IUserRepository _userRepository;

  public JwtService(IOptions<JwtSettings> jwtSettings, IUserRepository userRepository)
  {
    _jwt = jwtSettings.Value;
    _userRepository = userRepository;
  }

  public static TokenValidationParameters GetTokenValidationParameters(JwtSettings jwtSettings)
  {
    return new()
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = GetSymmetricSecurityKey(jwtSettings),
      ValidateIssuer = true,
      ValidIssuer = jwtSettings.Issuer,
      ValidateAudience = true,
      ValidAudience = jwtSettings.Audience,
    };
  }

  private static SymmetricSecurityKey GetSymmetricSecurityKey(JwtSettings jwtSettings)
  {
    return new(Encoding.UTF8.GetBytes(jwtSettings.Key));
  }

  public async Task<LoginResponse> Login(string username, string password)
  {
    var user = await _userRepository.GetByUsername(username);
    if (user == null || !user.VerifyPassword(password))
    {
      throw new UnauthorizedAccessException("Invalid credentials");
    }

    var token = CreateToken(user);
    return new LoginResponse { Token = token };
  }

  public string CreateToken(User user)
  {
    // Generate JWT token
    var claims = new[]
    {
      new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new Claim(ClaimTypes.Name, user.Username),
      new Claim(ClaimTypes.Role, user.Role),
    };
    var creds = new SigningCredentials(
      GetSymmetricSecurityKey(_jwt),
      SecurityAlgorithms.HmacSha256
    );

    var handler = new JsonWebTokenHandler();
    var token = handler.CreateToken(
      new SecurityTokenDescriptor
      {
        Issuer = _jwt.Issuer,
        Audience = _jwt.Audience,
        Claims = claims.ToDictionary(c => c.Type, c => (object)c.Value),
        Expires = DateTime.UtcNow.AddMinutes(_jwt.ExpiresInMinutes),
        SigningCredentials = creds,
      }
    );

    return token;
  }
}
