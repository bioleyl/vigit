namespace Api.Models.Responses;

public class UserResponse
{
  public int Id { get; set; }
  public string Username { get; set; } = null!;
  public string Role { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
}
