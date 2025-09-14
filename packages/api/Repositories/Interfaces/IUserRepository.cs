using Api.Entities;

namespace Api.Repositories.Interfaces;

public interface IUserRepository
{
  public List<User> GetAll();
  public User? GetById(int id);
  public User? GetByUsername(string username);
  public void Add(User user);
  public void Update(User user);
  public void Delete(User user);
}
