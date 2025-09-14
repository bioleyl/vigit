using Api.Entities;

namespace Api.Models.Responses;

public class RepositoryWithCollaboratorsResponse : RepositoryResponse
{
  public List<UserRepositoryResponse> Collaborators { get; set; } = [];

  public RepositoryWithCollaboratorsResponse() { }

  public RepositoryWithCollaboratorsResponse(Repository repository)
    : base(repository)
  {
    Collaborators = [.. repository.Collaborators.Select(ur => new UserRepositoryResponse(ur))];
  }
}
