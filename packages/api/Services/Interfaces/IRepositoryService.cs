using Api.Models.Requests;
using Api.Models.Responses;

namespace Api.Services.Interfaces;

public interface IRepositoryService
{
  public Task<RepositoryWithCollaboratorsResponse?> GetById(int id);
  public Task<List<RepositoryResponse>> GetByOwnerId(int ownerId);
  public Task<RepositoryResponse> Create(CreateRepositoryRequest request, int ownerId);
  public Task<RepositoryResponse> Update(
    int idToUpdate,
    UpdateRepositoryRequest request,
    int requestingUserId,
    bool requestingUserIsAdmin
  );
  public Task Delete(int idToDelete, int requestingUserId, bool requestingUserIsAdmin);

  public Task AddCollaborator(
    int id,
    int userIdToAdd,
    int requestingUserId,
    bool requestingUserIsAdmin
  );
  public Task RemoveCollaborator(
    int id,
    int userIdToRemove,
    int requestingUserId,
    bool requestingUserIsAdmin
  );
}
