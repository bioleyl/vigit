namespace Api.Models.Requests;

public class CreateRepositoryRequest
{
  public string Name { get; set; } = default!;
  public string Description { get; set; } = string.Empty;
}
