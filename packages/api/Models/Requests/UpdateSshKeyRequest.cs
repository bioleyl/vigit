namespace Api.Models.Requests;

public class UpdateSshKeyRequest
{
  public string Title { get; set; } = null!;
  public string PublicKey { get; set; } = null!;
}
