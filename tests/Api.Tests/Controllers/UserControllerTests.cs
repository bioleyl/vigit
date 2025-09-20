using System.Net.Http.Json;
using Api.Models.Requests;
using Api.Models.Responses;
using Api.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Api.Tests.Controllers;

public class UserControllerTests : IntegrationTestBase
{
  private const string UsersUrl = "/api/users";

  public UserControllerTests(CustomWebApplicationFactory factory)
    : base(factory) { }

  [Fact]
  public async Task GetAll_ReturnsAllUsers()
  {
    var response = await _client.GetAsync(UsersUrl);
    response.EnsureSuccessStatusCode();

    var users = await response.Content.ReadFromJsonAsync<UserResponse[]>();
    Assert.NotNull(users);
    Assert.Equal(2, users.Length);
  }

  [Fact]
  public async Task GetById_ExistingUser_ReturnsUser()
  {
    var response = await _client.GetAsync($"{UsersUrl}/1");
    response.EnsureSuccessStatusCode();

    var user = await response.Content.ReadFromJsonAsync<UserResponse>();
    Assert.NotNull(user);
    Assert.Equal("admin", user.Username);
  }

  [Fact]
  public async Task GetById_NonexistentUser_ReturnsNotFound()
  {
    var response = await _client.GetAsync($"{UsersUrl}/999");
    Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
  }

  [Fact]
  public async Task CreateUser_ValidRequest_ReturnsCreatedUser()
  {
    CreateUserRequest request = new()
    {
      Username = "newuser",
      Password = "password123",
      Role = "User",
    };
    var response = await _client.PostAsJsonAsync(UsersUrl, request);
    response.EnsureSuccessStatusCode();

    var user = await response.Content.ReadFromJsonAsync<UserResponse>();
    Assert.NotNull(user);
    Assert.Equal(request.Username, user.Username);
    Assert.Equal(request.Role, user.Role);

    var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Username == request.Username);
    Assert.NotNull(userInDb);
  }

  [Fact]
  public async Task UpdateUser_ExistingUser_ReturnsUpdatedUser()
  {
    UpdateUserRequest request = new() { Username = "admin_updated" };
    var response = await _client.PutAsJsonAsync($"{UsersUrl}/1", request);
    response.EnsureSuccessStatusCode();

    var updatedUser = await response.Content.ReadFromJsonAsync<UserResponse>();
    Assert.NotNull(updatedUser);
    Assert.Equal(request.Username, updatedUser.Username);

    var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Username == request.Username);
    Assert.NotNull(userInDb);
  }

  [Fact]
  public async Task DeleteUser_ExistingUser_ReturnsNoContent()
  {
    var response = await _client.DeleteAsync($"{UsersUrl}/2");
    Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);

    var getResponse = await _client.GetAsync($"{UsersUrl}/2");
    Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);

    var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Id == 2);
    Assert.Null(userInDb);
  }
}
