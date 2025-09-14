using Api.Entities;

namespace Api.Repositories.Interfaces;

public interface IRepositoryRepository
{
  public Repository? GetById(int id);
  public List<Repository> GetAll();
  public List<Repository> GetByOwnerId(int ownerId);
  public void Add(Repository repository);
  public void Update(Repository repository);
  public void Delete(Repository repository);

  // Collaborators
  public void AddCollaborator(Entities.UserRepository userRepository);
  public void RemoveCollaborator(Entities.UserRepository userRepository);
  public List<User> GetCollaborators(int repositoryId);
}
