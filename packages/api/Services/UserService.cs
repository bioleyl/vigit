using Api.Entities;
using Api.Models.Requests;
using Api.Models.Responses;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services;

public class UserService : IUserService
{
  private const string UserNotFoundMessage = "User not found";
  private readonly IUserRepository _repo;

  public UserService(IUserRepository repo)
  {
    _repo = repo;
  }

  public async Task<List<UserResponse>> GetAll()
  {
    var users = await _repo.GetAll();
    return users.Select(u => new UserResponse(u)).ToList();
  }

  public async Task<UserResponse?> GetById(int id)
  {
    var user = await _repo.GetById(id);
    return user == null ? null : new UserResponse(user);
  }

  public async Task<UserResponse> Create(CreateUserRequest request, bool requestingUserIsAdmin)
  {
    // Check if username already exists
    if (await _repo.GetByUsername(request.Username) != null)
      throw new ArgumentException("Username already exists");

    // Role is default to "User" if not provided by admin
    var role = requestingUserIsAdmin && !string.IsNullOrEmpty(request.Role) ? request.Role : "User";

    var user = new User(username: request.Username, password: request.Password, role: role);
    await _repo.Add(user);
    return new UserResponse(user);
  }

  public async Task<UserResponse> Update(
    int idToUpdate,
    UpdateUserRequest request,
    int requestingUserId,
    bool requestingUserIsAdmin
  )
  {
    var user =
      await _repo.GetById(idToUpdate) ?? throw new KeyNotFoundException(UserNotFoundMessage);

    // Check permissions
    if (requestingUserId != idToUpdate && !requestingUserIsAdmin)
      throw new UnauthorizedAccessException("You do not have permission to update this user");

    // Check username uniqueness if it's being changed
    if (!string.IsNullOrEmpty(request.Username) && request.Username != user.Username)
    {
      if (await _repo.GetByUsername(request.Username) != null)
        throw new ArgumentException("Username already exists");
      user.Username = request.Username;
    }

    // Update password if provided
    if (!string.IsNullOrEmpty(request.Password))
      user.SetPassword(request.Password);

    // Update role if provided and requester is admin
    if (!string.IsNullOrEmpty(request.Role))
    {
      if (!requestingUserIsAdmin)
        throw new UnauthorizedAccessException("Only admins can change roles");
      user.Role = request.Role;
    }

    await _repo.Update(user);
    return new UserResponse(user);
  }

  public async Task Delete(int idToDelete, int requestingUserId, bool requestingUserIsAdmin)
  {
    var user =
      await _repo.GetById(idToDelete) ?? throw new KeyNotFoundException(UserNotFoundMessage);

    // Check permissions
    if (requestingUserId != idToDelete && !requestingUserIsAdmin)
      throw new UnauthorizedAccessException("You do not have permission to delete this user");

    await _repo.Delete(user);
  }
}
