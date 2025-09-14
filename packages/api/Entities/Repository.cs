namespace Api.Entities;

public class Repository
{
  public int Id { get; set; }
  public string Name { get; set; } = default!;
  public string Description { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  // Owner
  public int OwnerId { get; set; }
  public User Owner { get; set; } = default!;

  // Collaborators (pivot table)
  public List<UserRepository> Collaborators { get; set; } = [];
}
