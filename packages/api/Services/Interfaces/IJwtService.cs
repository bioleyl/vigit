using System.Security.Claims;
using Api.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services.Interfaces;

public interface IJwtService
{
  public string CreateToken(User user);
  public SymmetricSecurityKey GetSymmetricSecurityKey();
  public TokenValidationParameters GetTokenValidationParameters();
}
