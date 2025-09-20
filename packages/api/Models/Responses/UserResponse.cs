namespace Api.Models.Responses;

public class UserResponse
{
  public int Id { get; set; }
  public string Username { get; set; } = default!;
  public string Role { get; set; } = default!;
  public DateTime CreatedAt { get; set; }

  public UserResponse() { }

  public UserResponse(int id, string username, string role, DateTime createdAt)
  {
    Id = id;
    Username = username;
    Role = role;
    CreatedAt = createdAt;
  }

  public UserResponse(Entities.User user)
  {
    Id = user.Id;
    Username = user.Username;
    Role = user.Role;
    CreatedAt = user.CreatedAt;
  }
}
