namespace Api.Models.Requests;

public class CreateUserRequest
{
  public string Username { get; set; } = default!;
  public string Password { get; set; } = default!;
  public string Role { get; set; } = "User";
}
