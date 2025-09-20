using Api.Data;
using Api.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class RepositoryRepository : IRepositoryRepository
{
  private readonly AppDbContext _db;

  public RepositoryRepository(AppDbContext db)
  {
    _db = db;
  }

  public Task<Repository?> GetById(int id)
  {
    return _db
      .Repositories.Include(r => r.Owner)
      .Include(r => r.Collaborators)
      .ThenInclude(ur => ur.User)
      .SingleOrDefaultAsync(r => r.Id == id);
  }

  public Task<List<Repository>> GetAll()
  {
    return _db.Repositories.Include(r => r.Owner).ToListAsync();
  }

  public Task<List<Repository>> GetByOwnerId(int ownerId)
  {
    return _db.Repositories.Where(r => r.OwnerId == ownerId).Include(r => r.Owner).ToListAsync();
  }

  public Task Add(Repository repository)
  {
    _db.Repositories.Add(repository);
    return _db.SaveChangesAsync();
  }

  public Task Update(Repository repository)
  {
    _db.Repositories.Update(repository);
    return _db.SaveChangesAsync();
  }

  public Task Delete(Repository repository)
  {
    _db.Repositories.Remove(repository);
    return _db.SaveChangesAsync();
  }

  // Collaborators
  public Task AddCollaborator(Entities.UserRepository userRepository)
  {
    _db.UserRepositories.Add(userRepository);
    return _db.SaveChangesAsync();
  }

  public Task RemoveCollaborator(Entities.UserRepository userRepository)
  {
    _db.UserRepositories.Remove(userRepository);
    return _db.SaveChangesAsync();
  }

  public Task<List<User>> GetCollaborators(int repositoryId)
  {
    return _db
      .UserRepositories.Where(ur => ur.RepositoryId == repositoryId)
      .Select(ur => ur.User)
      .ToListAsync();
  }
}
