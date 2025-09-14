using System.Security.Claims;
using System.Text;
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

  public string CreateToken(IEnumerable<Claim> claims)
  {
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
