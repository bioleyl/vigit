namespace Api.Entities;

public class SshKey
{
  public int Id { get; set; }
  public string Title { get; set; } = null!;
  public string PublicKey { get; set; } = null!;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public int UserId { get; set; }
  public User User { get; set; } = null!;

  protected SshKey() { }

  public SshKey(int userId, string title, string publicKey)
  {
    UserId = userId;
    Title = title;
    PublicKey = publicKey;
    CreatedAt = DateTime.UtcNow;
  }
}
