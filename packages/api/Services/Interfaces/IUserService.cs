using Api.Models.Requests;
using Api.Models.Responses;

namespace Api.Services.Interfaces;

public interface IUserService
{
  public Task<List<UserResponse>> GetAllAsync();
  public Task<UserResponse?> GetByIdAsync(int id);
  public Task<UserResponse> CreateAsync(CreateUserRequest request);
  public Task<UserResponse?> UpdateAsync(int id, UpdateUserRequest request);
  public Task<bool> DeleteAsync(int id);
}
