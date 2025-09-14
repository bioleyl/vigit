using Api.Entities;
using Api.Models.Requests;
using Api.Models.Responses;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services;

public class UserService : IUserService
{
  private readonly IUserRepository _repo;

  public UserService(IUserRepository repo)
  {
    _repo = repo;
  }

  public List<UserResponse> GetAll()
  {
    return [.. _repo.GetAll().Select(u => u.ToResponse())];
  }

  public UserResponse? GetById(int id)
  {
    var user = _repo.GetById(id);
    return user?.ToResponse();
  }

  public UserResponse Create(CreateUserRequest request, bool requestingUserIsAdmin)
  {
    // Check if username already exists
    if (_repo.GetByUsername(request.Username) != null)
      throw new ArgumentException("Username already exists");

    // Role is default to "User" if not provided by admin
    var role = requestingUserIsAdmin && !string.IsNullOrEmpty(request.Role) ? request.Role : "User";

    var user = new User { Username = request.Username, Role = role };
    user.SetPassword(request.Password);
    _repo.Add(user);
    return user.ToResponse();
  }

  public UserResponse? Update(
    int idToUpdate,
    UpdateUserRequest request,
    int requestingUserId,
    bool requestingUserIsAdmin
  )
  {
    var user = _repo.GetById(idToUpdate) ?? throw new KeyNotFoundException("User not found");

    // Check permissions
    if (requestingUserId != idToUpdate && !requestingUserIsAdmin)
      throw new UnauthorizedAccessException("You do not have permission to update this user");

    // Check username uniqueness if it's being changed
    if (!string.IsNullOrEmpty(request.Username) && request.Username != user.Username)
    {
      if (_repo.GetByUsername(request.Username) != null)
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

    _repo.Update(user);
    return user.ToResponse();
  }

  public void Delete(int idToDelete, int requestingUserId, bool requestingUserIsAdmin)
  {
    var user = _repo.GetById(idToDelete) ?? throw new KeyNotFoundException("User not found");

    // Check permissions
    if (requestingUserId != idToDelete && !requestingUserIsAdmin)
      throw new UnauthorizedAccessException("You do not have permission to delete this user");

    _repo.Delete(user);
  }
}
