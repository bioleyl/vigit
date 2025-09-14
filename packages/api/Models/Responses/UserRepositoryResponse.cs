namespace Api.Models.Responses;

public class UserRepositoryResponse
{
  public int UserId { get; set; }
  public string Username { get; set; } = default!;
  public string Role { get; set; } = default!;

  public UserRepositoryResponse() { }

  public UserRepositoryResponse(Entities.UserRepository userRepository)
  {
    UserId = userRepository.User.Id;
    Username = userRepository.User.Username;
    Role = userRepository.Role;
  }
}
