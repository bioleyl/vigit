using Api.Entities;

namespace Api.Repositories.Interfaces;

public interface IUserRepository
{
  public Task<List<User>> GetAll();
  public Task<User?> GetById(int userId);
  public Task<User?> GetByUsername(string username);
  public Task Add(User user);
  public Task Update(User user);
  public Task Delete(User user);
}
