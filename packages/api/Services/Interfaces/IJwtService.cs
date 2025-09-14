using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services.Interfaces;

public interface IJwtService
{
  public string CreateToken(IEnumerable<Claim> claims);
  public SymmetricSecurityKey GetSymmetricSecurityKey();
  public TokenValidationParameters GetTokenValidationParameters();
}
