using Api.Models.Requests;
using Api.Models.Responses;

namespace Api.Services.Interfaces;

public interface IUserService
{
  public Task<List<UserResponse>> GetAll();
  public Task<UserResponse?> GetById(int id);
  public Task<UserResponse> Create(CreateUserRequest request, bool requestingUserIsAdmin);
  public Task<UserResponse> Update(
    int idToUpdate,
    UpdateUserRequest request,
    int requestingUserId,
    bool requestingUserIsAdmin
  );
  public Task Delete(int idToDelete, int requestingUserId, bool requestingUserIsAdmin);
}
