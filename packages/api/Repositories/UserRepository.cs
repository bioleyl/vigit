using Api.Data;
using Api.Entities;
using Api.Repositories.Interfaces;

namespace Api.Repositories;

public class UserRepository : IUserRepository
{
  private readonly AppDbContext _db;

  public UserRepository(AppDbContext db)
  {
    _db = db;
  }

  public List<User> GetAll()
  {
    return [.. _db.Users];
  }

  public User? GetById(int id)
  {
    return _db.Users.SingleOrDefault(u => u.Id == id);
  }

  public User? GetByUsername(string username)
  {
    return _db.Users.SingleOrDefault(u => u.Username == username);
  }

  public void Add(User user)
  {
    _db.Users.Add(user);
    _db.SaveChanges();
  }

  public void Update(User user)
  {
    _db.Users.Update(user);
    _db.SaveChanges();
  }

  public void Delete(User user)
  {
    _db.Users.Remove(user);
    _db.SaveChanges();
  }
}
