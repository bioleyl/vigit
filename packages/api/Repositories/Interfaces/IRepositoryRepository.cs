using Api.Entities;

namespace Api.Repositories.Interfaces;

public interface IRepositoryRepository
{
  public Task<Repository?> GetById(int id);
  public Task<Repository?> GetByName(string name);
  public Task<List<Repository>> GetAll();
  public Task<List<Repository>> GetByOwnerId(int ownerId);
  public Task Add(Repository repository);
  public Task Update(Repository repository);
  public Task Delete(Repository repository);

  // Collaborators
  public Task AddCollaborator(Entities.UserRepository userRepository);
  public Task RemoveCollaborator(Entities.UserRepository userRepository);
  public Task<List<User>> GetCollaborators(int repositoryId);
}
