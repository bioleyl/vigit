using Api.Entities;
using Api.Models.Requests;
using Api.Models.Responses;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services;

public class RepositoryService : IRepositoryService
{
  private const string RepositoryNotFoundMessage = "Repository not found";
  private readonly IRepositoryRepository _repo;

  public RepositoryService(IRepositoryRepository repo)
  {
    _repo = repo;
  }

  public RepositoryWithCollaboratorsResponse? GetById(int id)
  {
    var repo = _repo.GetById(id);
    return repo == null ? null : new RepositoryWithCollaboratorsResponse(repo);
  }

  public List<RepositoryResponse> GetByOwnerId(int ownerId)
  {
    return [.. _repo.GetByOwnerId(ownerId).Select(r => new RepositoryResponse(r))];
  }

  public RepositoryResponse Create(CreateRepositoryRequest request, int ownerId)
  {
    var existing = _repo.GetByOwnerId(ownerId);
    if (existing.Any(r => r.Name == request.Name))
      throw new ArgumentException("Repository name already exists for this user");

    var repo = new Repository
    {
      Name = request.Name,
      Description = request.Description,
      OwnerId = ownerId,
    };

    _repo.Add(repo);

    return new RepositoryResponse(repo);
  }

  public RepositoryResponse Update(
    int idToUpdate,
    UpdateRepositoryRequest request,
    int requestingUserId,
    bool requestingUserIsAdmin
  )
  {
    var repo =
      _repo.GetById(idToUpdate) ?? throw new KeyNotFoundException(RepositoryNotFoundMessage);

    // Check permissions
    if (repo.OwnerId != requestingUserId && !requestingUserIsAdmin)
      throw new UnauthorizedAccessException("You do not have permission to update this repository");

    // Check name uniqueness if it's being changed
    if (!string.IsNullOrEmpty(request.Name) && request.Name != repo.Name)
    {
      var existing = _repo.GetByOwnerId(repo.OwnerId);
      if (existing.Any(r => r.Name == request.Name))
        throw new ArgumentException("Repository name already exists for this user");
      repo.Name = request.Name;
    }

    // Update description if provided
    if (request.Description != null)
      repo.Description = request.Description;

    _repo.Update(repo);
    return new RepositoryResponse(repo);
  }

  public void Delete(int idToDelete, int requestingUserId, bool requestingUserIsAdmin)
  {
    var repo =
      _repo.GetById(idToDelete) ?? throw new KeyNotFoundException(RepositoryNotFoundMessage);

    // Check permissions
    if (repo.OwnerId != requestingUserId && !requestingUserIsAdmin)
      throw new UnauthorizedAccessException("You do not have permission to delete this repository");

    _repo.Delete(repo);
  }

  public void AddCollaborator(
    int id,
    int userIdToAdd,
    int requestingUserId,
    bool requestingUserIsAdmin
  )
  {
    var repo = _repo.GetById(id) ?? throw new KeyNotFoundException(RepositoryNotFoundMessage);

    // Check permissions
    if (repo.OwnerId != requestingUserId && !requestingUserIsAdmin)
      throw new UnauthorizedAccessException("You do not have permission to modify this repository");

    // Check if user is already a collaborator
    if (repo.Collaborators.Any(ur => ur.UserId == userIdToAdd))
      throw new ArgumentException("User is already a collaborator");

    var userRepository = new UserRepository { UserId = userIdToAdd, RepositoryId = id };
    _repo.AddCollaborator(userRepository);
  }

  public void RemoveCollaborator(
    int id,
    int userIdToRemove,
    int requestingUserId,
    bool requestingUserIsAdmin
  )
  {
    var repo = _repo.GetById(id) ?? throw new KeyNotFoundException(RepositoryNotFoundMessage);

    // Check permissions
    if (repo.OwnerId != requestingUserId && !requestingUserIsAdmin)
      throw new UnauthorizedAccessException("You do not have permission to modify this repository");

    // Check if user is a collaborator
    var userRepository =
      repo.Collaborators.SingleOrDefault(ur => ur.UserId == userIdToRemove)
      ?? throw new KeyNotFoundException("User is not a collaborator");

    _repo.RemoveCollaborator(userRepository);
  }
}
