using System.Security.Claims;
using System.Text;
using Api.Entities;
using Api.Services.Interfaces;
using Api.Settings;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services;

public class JwtService : IJwtService
{
  private readonly JwtSettings _jwt;

  public JwtService(JwtSettings jwtSettings)
  {
    _jwt = jwtSettings;
  }

  public SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(_jwt.Key));

  public TokenValidationParameters GetTokenValidationParameters() =>
    new()
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = GetSymmetricSecurityKey(),
      ValidateIssuer = true,
      ValidIssuer = _jwt.Issuer,
      ValidateAudience = true,
      ValidAudience = _jwt.Audience,
    };

  public string CreateToken(User user)
  {
    // Generate JWT token
    var claims = new[]
    {
      new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new Claim(ClaimTypes.Name, user.Username),
      new Claim(ClaimTypes.Role, user.Role),
    };
    var creds = new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);

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
