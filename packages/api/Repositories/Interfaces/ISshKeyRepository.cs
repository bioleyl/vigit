using Api.Entities;

namespace Api.Repositories.Interfaces;

public interface ISshKeyRepository
{
  public Task<SshKey?> GetById(int id);
  public Task<List<SshKey>> GetByOwnerId(int ownerId);
  public Task<SshKey?> GetByBlob(string blob);
  public Task Add(SshKey key);
  public Task Update(SshKey key);
  public Task Delete(SshKey key);
}
