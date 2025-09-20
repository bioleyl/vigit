namespace Api.Models.Requests;

public class CreateSshKeyRequest
{
  public string Title { get; set; } = null!;
  public string PublicKey { get; set; } = null!;
}
