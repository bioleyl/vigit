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

  public Repository? GetById(int id)
  {
    return _db
      .Repositories.Include(r => r.Owner)
      .Include(r => r.Collaborators)
      .ThenInclude(ur => ur.User)
      .SingleOrDefault(r => r.Id == id);
  }

  public List<Repository> GetAll()
  {
    return [.. _db.Repositories.Include(r => r.Owner)];
  }

  public List<Repository> GetByOwnerId(int ownerId)
  {
    return [.. _db.Repositories.Where(r => r.OwnerId == ownerId).Include(r => r.Owner)];
  }

  public void Add(Repository repository)
  {
    _db.Repositories.Add(repository);
    _db.SaveChanges();
  }

  public void Update(Repository repository)
  {
    _db.Repositories.Update(repository);
    _db.SaveChanges();
  }

  public void Delete(Repository repository)
  {
    _db.Repositories.Remove(repository);
    _db.SaveChanges();
  }

  // Collaborators
  public void AddCollaborator(Entities.UserRepository userRepository)
  {
    _db.UserRepositories.Add(userRepository);
    _db.SaveChanges();
  }

  public void RemoveCollaborator(Entities.UserRepository userRepository)
  {
    _db.UserRepositories.Remove(userRepository);
    _db.SaveChanges();
  }

  public List<User> GetCollaborators(int repositoryId)
  {
    return
    [
      .. _db.UserRepositories.Where(ur => ur.RepositoryId == repositoryId).Select(ur => ur.User),
    ];
  }
}
