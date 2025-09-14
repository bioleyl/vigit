using System.Net.Http.Headers;
using System.Security.Claims;
using Api.Services.Interfaces;

namespace Api.Tests.Helpers;

public static class TestAuthHelper
{
  public static void AddJwtToken(
    this HttpClient client,
    IJwtService jwtService,
    string username,
    string role
  )
  {
    var claims = new[] { new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.Role, role) };
    var token = jwtService.CreateToken(claims);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
  }
}
