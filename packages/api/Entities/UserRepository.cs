namespace Api.Entities;

public class UserRepository
{
  public int UserId { get; set; }
  public User User { get; set; } = default!;

  public int RepositoryId { get; set; }
  public Repository Repository { get; set; } = default!;

  public string Role { get; set; } = default!;
}
