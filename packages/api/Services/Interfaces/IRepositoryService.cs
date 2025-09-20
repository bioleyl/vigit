using System.Security.Claims;
using Api.Models.Requests;
using Api.Models.Responses;

namespace Api.Services.Interfaces;

public interface IRepositoryService
{
  public Task<RepositoryWithCollaboratorsResponse> GetById(int id, ClaimsPrincipal requester);
  public Task<List<RepositoryResponse>> GetByOwnerId(int ownerId, ClaimsPrincipal requester);
  public Task<RepositoryResponse> Create(
    CreateRepositoryRequest request,
    int ownerId,
    ClaimsPrincipal requester
  );
  public Task<RepositoryResponse> Update(
    int idToUpdate,
    UpdateRepositoryRequest request,
    ClaimsPrincipal requester
  );
  public Task Delete(int idToDelete, ClaimsPrincipal requester);

  public Task AddCollaborator(int id, int userIdToAdd, ClaimsPrincipal requester);
  public Task RemoveCollaborator(int id, int userIdToRemove, ClaimsPrincipal requester);
}
