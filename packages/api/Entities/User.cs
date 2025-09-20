using Api.Models.Enums;

namespace Api.Entities;

public class User
{
  public int Id { get; set; }

  public string Username { get; set; } = null!;
  public string PasswordHash { get; set; } = null!;
  public string Role { get; set; } = UserRole.User;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public List<Repository> OwnedRepositories { get; set; } = [];
  public List<UserRepository> Collaborations { get; set; } = [];
  public List<SshKey> SshKeys { get; set; } = [];

  protected User() { }

  public User(string username, string password, string role = UserRole.User)
  {
    Username = username;
    SetPassword(password);
    Role = role;
    CreatedAt = DateTime.UtcNow;
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
