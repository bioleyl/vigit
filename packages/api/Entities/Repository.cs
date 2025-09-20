namespace Api.Entities;

public class Repository
{
  public int Id { get; set; }

  public string Name { get; set; } = null!;
  public string Description { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  // Owner
  public int OwnerId { get; set; }
  public User Owner { get; set; } = null!;

  // Collaborators (pivot table)
  public List<UserRepository> Collaborators { get; set; } = [];

  protected Repository() { }

  public Repository(string name, int ownerId, string description = "")
  {
    Name = name;
    OwnerId = ownerId;
    Description = description;
    CreatedAt = DateTime.UtcNow;
  }
}
