using Api.Entities;

namespace Api.Models.Responses;

public class SshKeyResponse
{
  public int Id { get; set; }
  public string Title { get; set; } = null!;
  public string PublicKey { get; set; } = null!;
  public DateTime CreatedAt { get; set; }

  public SshKeyResponse() { }

  public SshKeyResponse(int id, string title, string publicKey, DateTime createdAt)
  {
    Id = id;
    Title = title;
    PublicKey = publicKey;
    CreatedAt = createdAt;
  }

  public SshKeyResponse(SshKey key)
    : this(key.Id, key.Title, key.PublicKey, key.CreatedAt) { }
}
