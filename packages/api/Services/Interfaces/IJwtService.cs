using Api.Entities;
using Api.Models.Responses;

namespace Api.Services.Interfaces;

public interface IJwtService
{
  public Task<LoginResponse> Login(string username, string password);
  public string CreateToken(User user);
}
