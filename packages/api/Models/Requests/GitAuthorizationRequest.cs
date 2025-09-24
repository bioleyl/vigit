namespace Api.Models.Requests;

public class GitAuthorizationRequest
{
  public string KeyBlob { get; set; } = string.Empty;
  public string RepositoryName { get; set; } = string.Empty;
  public string Action { get; set; } = string.Empty;
}
