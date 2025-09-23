using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Entities;

public class SshKey
{
  public int Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Type { get; set; } = string.Empty;
  public string Blob { get; set; } = string.Empty;
  public string Comment { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public int UserId { get; set; }
  public User User { get; set; } = null!;

  [NotMapped]
  public string PublicKey
  {
    get => $"{Type} {Blob} {Comment}".Trim();
    set => UpdateFromPublicKey(value);
  }

  protected SshKey() { }

  public SshKey(int userId, string title, string publicKey)
  {
    UpdateFromPublicKey(publicKey);

    UserId = userId;
    Title = title;
    CreatedAt = DateTime.UtcNow;
  }

  private void UpdateFromPublicKey(string publicKey)
  {
    if (string.IsNullOrWhiteSpace(publicKey))
      throw new ArgumentException("Public key cannot be null or empty.", nameof(publicKey));

    // Split on whitespace (type, blob, comment?)
    var parts = publicKey.Trim().Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);

    if (parts.Length < 2)
      throw new ArgumentException("Invalid SSH public key format.", nameof(publicKey));

    Type = parts[0];
    Blob = parts[1];
    Comment = parts.Length > 2 ? parts[2] : string.Empty;
  }
}
