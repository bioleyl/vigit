using System.Security.Claims;
using Api.Entities;
using Api.Extensions;
using Api.Models.Requests;
using Api.Models.Responses;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services;

public class RepositoryService : IRepositoryService
{
  private const string RepositoryNotFoundMessage = "Repository not found";
  private readonly IRepositoryRepository _repo;
  private readonly IGitRepository _gitRepository;

  public RepositoryService(IRepositoryRepository repo, IGitRepository gitRepository)
  {
    _repo = repo;
    _gitRepository = gitRepository;
  }

  public async Task<RepositoryWithCollaboratorsResponse> GetById(int id, ClaimsPrincipal requester)
  {
    var repo = await _repo.GetById(id) ?? throw new KeyNotFoundException(RepositoryNotFoundMessage);

    // Check permissions
    if (requester.GetUserId() != repo.OwnerId && !requester.IsAdmin())
      throw new UnauthorizedAccessException("You do not have permission to view this repository");

    return new RepositoryWithCollaboratorsResponse(repo);
  }

  public async Task<List<RepositoryResponse>> GetByOwnerId(int ownerId, ClaimsPrincipal requester)
  {
    if (requester.GetUserId() != ownerId && !requester.IsAdmin())
    {
      throw new UnauthorizedAccessException(
        "You do not have permission to view these repositories"
      );
    }

    var repos = await _repo.GetByOwnerId(ownerId);
    return repos.Select(r => new RepositoryResponse(r)).ToList();
  }

  public async Task<RepositoryResponse> Create(
    CreateRepositoryRequest request,
    int ownerId,
    ClaimsPrincipal requester
  )
  {
    // Check permissions
    if (ownerId != requester.GetUserId() && !requester.IsAdmin())
      throw new UnauthorizedAccessException("You do not have permission to update this repository");

    var existing = await _repo.GetByOwnerId(ownerId);
    if (existing.Any(r => r.Name == request.Name))
      throw new ArgumentException("Repository name already exists for this user", nameof(request));

    var repo = new Repository(
      name: request.Name,
      description: request.Description,
      ownerId: ownerId
    );

    // Initialize bare git repository
    _gitRepository.InitBareRepository(repo.Name);

    await _repo.Add(repo);

    return new RepositoryResponse(repo);
  }

  public async Task<RepositoryResponse> Update(
    int idToUpdate,
    UpdateRepositoryRequest request,
    ClaimsPrincipal requester
  )
  {
    var repo =
      await _repo.GetById(idToUpdate) ?? throw new KeyNotFoundException(RepositoryNotFoundMessage);

    // Check permissions
    if (repo.OwnerId != requester.GetUserId() && !requester.IsAdmin())
      throw new UnauthorizedAccessException("You do not have permission to update this repository");

    // Check name uniqueness if it's being changed
    if (!string.IsNullOrEmpty(request.Name) && request.Name != repo.Name)
    {
      var existing = await _repo.GetByOwnerId(repo.OwnerId);
      if (existing.Any(r => r.Name == request.Name))
      {
        throw new ArgumentException(
          "Repository name already exists for this user",
          nameof(request)
        );
      }

      repo.Name = request.Name;
    }

    // Update description if provided
    if (request.Description != null)
      repo.Description = request.Description;

    await _repo.Update(repo);
    return new RepositoryResponse(repo);
  }

  public async Task Delete(int idToDelete, ClaimsPrincipal requester)
  {
    var repo =
      await _repo.GetById(idToDelete) ?? throw new KeyNotFoundException(RepositoryNotFoundMessage);

    // Check permissions
    if (repo.OwnerId != requester.GetUserId() && !requester.IsAdmin())
      throw new UnauthorizedAccessException("You do not have permission to delete this repository");

    await _repo.Delete(repo);
  }

  public async Task AddCollaborator(int id, int userIdToAdd, ClaimsPrincipal requester)
  {
    var repo = await _repo.GetById(id) ?? throw new KeyNotFoundException(RepositoryNotFoundMessage);

    // Check permissions
    if (repo.OwnerId != requester.GetUserId() && !requester.IsAdmin())
      throw new UnauthorizedAccessException("You do not have permission to modify this repository");

    // Check if user is already a collaborator
    if (repo.Collaborators.Any(ur => ur.UserId == userIdToAdd))
      throw new ArgumentException("User is already a collaborator", nameof(userIdToAdd));

    var userRepository = new UserRepository(userId: userIdToAdd, repositoryId: id);
    await _repo.AddCollaborator(userRepository);
  }

  public async Task RemoveCollaborator(int id, int userIdToRemove, ClaimsPrincipal requester)
  {
    var repo = await _repo.GetById(id) ?? throw new KeyNotFoundException(RepositoryNotFoundMessage);

    // Check permissions
    if (repo.OwnerId != requester.GetUserId() && !requester.IsAdmin())
      throw new UnauthorizedAccessException("You do not have permission to modify this repository");

    // Check if user is a collaborator
    var userRepository =
      repo.Collaborators.SingleOrDefault(ur => ur.UserId == userIdToRemove)
      ?? throw new KeyNotFoundException("User is not a collaborator");

    await _repo.RemoveCollaborator(userRepository);
  }
}
