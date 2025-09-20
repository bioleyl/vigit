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
    User user = new(username: username, password: "test", role: role) { Id = 1 };
    var token = jwtService.CreateToken(user);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
  }
}
