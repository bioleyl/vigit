using Api.Models.Requests;
using Api.Models.Responses;

namespace Api.Entities;

public class User
{
  public int Id { get; set; }
  public string Username { get; set; } = null!;
  public string PasswordHash { get; set; } = null!;
  public string Role { get; set; } = "User"; // default role
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  // Owned repositories
  public List<Repository> OwnedRepositories { get; set; } = [];

  // Collaborations
  public List<UserRepository> Collaborations { get; set; } = [];

  public User() { }

  public User(CreateUserRequest request)
  {
    Username = request.Username;
    Role = request.Role;
    SetPassword(request.Password);
  }

  public void SetPassword(string password)
  {
    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
  }

  public bool VerifyPassword(string password)
  {
    return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
  }
}
