using System.Net.Http.Json;
using System.Text.Json;
using Api.Models.Requests;
using Api.Models.Responses;
using Api.Tests.Helpers;

namespace Api.Tests.Controllers;

public class AuthControllerTests : IntegrationTestBase
{
  private const string LoginUrl = "/api/auth/login";

  public AuthControllerTests(CustomWebApplicationFactory factory)
    : base(factory) { }

  [Fact]
  public async Task Login_WithValidUser_ReturnsOk()
  {
    LoginRequest request = new() { Username = "admin", Password = "admin" };
    var response = await _client.PostAsJsonAsync(LoginUrl, request);
    response.EnsureSuccessStatusCode();

    var content = await response.Content.ReadFromJsonAsync<LoginResponse>();
    Assert.NotNull(content?.Token);
  }

  [Fact]
  public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
  {
    LoginRequest request = new() { Username = "admin", Password = "wrongpassword" };
    var response = await _client.PostAsJsonAsync(LoginUrl, request);
    Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);

    var content = await response.Content.ReadFromJsonAsync<JsonElement>();
    var message = content.GetProperty("message").GetString();
    Assert.Equal("Invalid credentials", message);
  }

  [Fact]
  public async Task Login_WithNonexistentUser_ReturnsUnauthorized()
  {
    LoginRequest request = new() { Username = "doesnotexist", Password = "password" };
    var response = await _client.PostAsJsonAsync(LoginUrl, request);
    Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);

    var content = await response.Content.ReadFromJsonAsync<JsonElement>();
    var message = content.GetProperty("message").GetString();
    Assert.Equal("Invalid credentials", message);
  }
}
