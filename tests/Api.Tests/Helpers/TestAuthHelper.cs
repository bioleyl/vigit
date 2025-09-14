using System.Net.Http.Headers;
using Api.Entities;
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
    User user = new()
    {
      Id = 1,
      Username = username,
      Role = role,
    };
    var token = jwtService.CreateToken(user);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
  }
}
