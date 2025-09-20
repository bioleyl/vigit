using Api.Data;
using Api.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class UserRepository : IUserRepository
{
  private readonly AppDbContext _db;

  public UserRepository(AppDbContext db)
  {
    _db = db;
  }

  public Task<List<User>> GetAll()
  {
    return _db.Users.ToListAsync();
  }

  public Task<User?> GetById(int userId)
  {
    return _db
      .Users.Include(u => u.OwnedRepositories)
      .Include(u => u.Collaborations)
      .Include(u => u.SshKeys)
      .SingleOrDefaultAsync(u => u.Id == userId);
  }

  public Task<User?> GetByUsername(string username)
  {
    return _db.Users.SingleOrDefaultAsync(u => u.Username == username);
  }

  public Task Add(User user)
  {
    _db.Users.Add(user);
    return _db.SaveChangesAsync();
  }

  public Task Update(User user)
  {
    _db.Users.Update(user);
    return _db.SaveChangesAsync();
  }

  public Task Delete(User user)
  {
    _db.Users.Remove(user);
    return _db.SaveChangesAsync();
  }
}
