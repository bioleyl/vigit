using System.Security.Claims;
using Api.Models.Requests;
using Api.Models.Responses;

namespace Api.Services.Interfaces;

public interface IUserService
{
  public Task<List<UserResponse>> GetAll();
  public Task<UserFullResponse> GetById(int id, ClaimsPrincipal requester);
  public Task<UserResponse> Create(CreateUserRequest request, ClaimsPrincipal requester);
  public Task<UserResponse> Update(
    int idToUpdate,
    UpdateUserRequest request,
    ClaimsPrincipal requester
  );
  public Task Delete(int idToDelete, ClaimsPrincipal requester);
}
