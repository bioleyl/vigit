using Api.Models.Requests;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services;

public class GitService : IGitService
{
  private readonly ISshKeyRepository _sshKeyRepository;
  private readonly IRepositoryRepository _repositoryRepository;

  public GitService(ISshKeyRepository sshKeyRepository, IRepositoryRepository repositoryRepository)
  {
    _sshKeyRepository = sshKeyRepository;
    _repositoryRepository = repositoryRepository;
  }

  public async Task<bool> IsAuthorized(GitAuthorizationRequest request)
  {
    var sshKey = await _sshKeyRepository.GetByBlob(request.KeyBlob);
    if (sshKey == null)
      return false;

    var repository = await _repositoryRepository.GetByName(request.RepositoryName);
    if (repository == null)
      return false;

    return repository.OwnerId == sshKey.UserId
      || repository.Collaborators.Any(c => c.UserId == sshKey.UserId);
  }
}
