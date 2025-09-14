using Api.Models.Requests;
using Api.Models.Responses;

namespace Api.Services.Interfaces;

public interface IRepositoryService
{
  public RepositoryWithCollaboratorsResponse? GetById(int id);
  public List<RepositoryResponse> GetByOwnerId(int ownerId);
  public RepositoryResponse Create(CreateRepositoryRequest request, int ownerId);
  public RepositoryResponse Update(
    int idToUpdate,
    UpdateRepositoryRequest request,
    int requestingUserId,
    bool requestingUserIsAdmin
  );
  public void Delete(int idToDelete, int requestingUserId, bool requestingUserIsAdmin);

  public void AddCollaborator(
    int id,
    int userIdToAdd,
    int requestingUserId,
    bool requestingUserIsAdmin
  );
  public void RemoveCollaborator(
    int id,
    int userIdToRemove,
    int requestingUserId,
    bool requestingUserIsAdmin
  );
}
