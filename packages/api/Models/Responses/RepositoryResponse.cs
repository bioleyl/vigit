using Api.Entities;

namespace Api.Models.Responses;

public class RepositoryResponse
{
  public int Id { get; set; }
  public string Name { get; set; } = default!;
  public string Description { get; set; } = default!;
  public DateTime CreatedAt { get; set; }
  public UserResponse Owner { get; set; } = default!;

  public RepositoryResponse() { }

  public RepositoryResponse(Repository repository)
  {
    Id = repository.Id;
    Name = repository.Name;
    Description = repository.Description;
    CreatedAt = repository.CreatedAt;
    Owner = new UserResponse(repository.Owner);
  }
}
