using Api.Data;
using Api.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class SshKeyRepository : ISshKeyRepository
{
  private readonly AppDbContext _db;

  public SshKeyRepository(AppDbContext db)
  {
    _db = db;
  }

  public Task<SshKey?> GetById(int id)
  {
    return _db.SshKeys.FindAsync(id).AsTask();
  }

  public Task<List<SshKey>> GetByOwnerId(int ownerId)
  {
    return _db.SshKeys.Where(k => k.UserId == ownerId).ToListAsync();
  }

  public Task Add(SshKey key)
  {
    _db.SshKeys.Add(key);
    return _db.SaveChangesAsync();
  }

  public Task Update(SshKey key)
  {
    _db.SshKeys.Update(key);
    return _db.SaveChangesAsync();
  }

  public Task Delete(SshKey key)
  {
    _db.SshKeys.Remove(key);
    return _db.SaveChangesAsync();
  }
}
