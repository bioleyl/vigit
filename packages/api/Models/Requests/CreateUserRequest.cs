namespace Api.Models.Requests;

public class CreateUserRequest
{
  public string Username { get; set; } = null!;
  public string Password { get; set; } = null!;
  public string Role { get; set; } = "User";
}
