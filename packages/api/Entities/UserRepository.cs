using Api.Models.Enums;

namespace Api.Entities;

public class UserRepository
{
  // Composite key properties
  public int UserId { get; set; }
  public User User { get; set; } = null!;

  public int RepositoryId { get; set; }
  public Repository Repository { get; set; } = null!;

  public string Role { get; set; } = UserRepositoryRole.Collaborator; // default role

  protected UserRepository() { }

  public UserRepository(int userId, int repositoryId, string role = UserRepositoryRole.Collaborator)
  {
    UserId = userId;
    RepositoryId = repositoryId;
    Role = role;
  }
}
