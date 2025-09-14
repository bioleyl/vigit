using Api.Models.Requests;
using Api.Models.Responses;

namespace Api.Services.Interfaces;

public interface IUserService
{
  public List<UserResponse> GetAll();
  public UserResponse? GetById(int id);
  public UserResponse Create(CreateUserRequest request, bool requestingUserIsAdmin);
  public UserResponse Update(
    int idToUpdate,
    UpdateUserRequest request,
    int requestingUserId,
    bool requestingUserIsAdmin
  );
  public void Delete(int idToDelete, int requestingUserId, bool requestingUserIsAdmin);
}
