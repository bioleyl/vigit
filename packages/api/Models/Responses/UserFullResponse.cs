using Api.Entities;

namespace Api.Models.Responses;

public class UserFullResponse : UserResponse
{
  public List<RepositoryWithCollaboratorsResponse> OwnedRepositories { get; set; } = [];
  public List<UserRepositoryResponse> Collaborations { get; set; } = [];
  public List<SshKeyResponse> SshKeys { get; set; } = [];

  public UserFullResponse() { }

  public UserFullResponse(User user)
    : base(user)
  {
    OwnedRepositories =
    [
      .. user.OwnedRepositories.Select(r => new RepositoryWithCollaboratorsResponse(r)),
    ];
    Collaborations = [.. user.Collaborations.Select(ur => new UserRepositoryResponse(ur))];
    SshKeys = [.. user.SshKeys.Select(k => new SshKeyResponse(k))];
  }
}
