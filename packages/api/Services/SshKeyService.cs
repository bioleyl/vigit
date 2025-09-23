using System.Security.Claims;
using Api.Entities;
using Api.Extensions;
using Api.Models.Requests;
using Api.Models.Responses;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services;

public class SshKeyService : ISshKeyService
{
  private readonly ISshKeyRepository _repo;

  public SshKeyService(ISshKeyRepository repo)
  {
    _repo = repo;
  }

  public async Task<SshKeyResponse> Create(
    CreateSshKeyRequest request,
    int ownerId,
    ClaimsPrincipal requester
  )
  {
    // Check permissions
    if (ownerId != requester.GetUserId() && !requester.IsAdmin())
    {
      throw new UnauthorizedAccessException(
        "You do not have permission to create an SSH key for this user"
      );
    }

    // Check title uniqueness
    var existing = await _repo.GetByOwnerId(ownerId);
    if (existing.Any(k => k.Title == request.Title))
      throw new ArgumentException("SSH key title already exists for this user", nameof(request));

    var key = new SshKey(title: request.Title, publicKey: request.PublicKey, userId: ownerId);
    await _repo.Add(key);
    return new SshKeyResponse(key);
  }

  public async Task<SshKeyResponse> GetById(int id, ClaimsPrincipal requester)
  {
    var key = await _repo.GetById(id) ?? throw new KeyNotFoundException("SSH key not found");

    // Check permissions
    if (key.UserId != requester.GetUserId() && !requester.IsAdmin())
      throw new UnauthorizedAccessException("You do not have permission to view this SSH key");

    return new SshKeyResponse(key);
  }

  public async Task<List<SshKeyResponse>> GetByOwnerId(int ownerId, ClaimsPrincipal requester)
  {
    // Check permissions
    if (ownerId != requester.GetUserId() && !requester.IsAdmin())
      throw new UnauthorizedAccessException("You do not have permission to view these SSH keys");

    var keys = await _repo.GetByOwnerId(ownerId);
    return keys.Select(k => new SshKeyResponse(k)).ToList();
  }

  public async Task<SshKeyResponse> Update(
    int idToUpdate,
    UpdateSshKeyRequest request,
    ClaimsPrincipal requester
  )
  {
    var key =
      await _repo.GetById(idToUpdate) ?? throw new KeyNotFoundException("SSH key not found");

    // Check permissions
    if (requester.GetUserId() != key.UserId && !requester.IsAdmin())
      throw new UnauthorizedAccessException("You do not have permission to update this SSH key");

    // Check title uniqueness if it's being changed
    if (!string.IsNullOrEmpty(request.Title) && request.Title != key.Title)
    {
      var existing = await _repo.GetByOwnerId(key.UserId);
      if (existing.Any(k => k.Title == request.Title))
        throw new ArgumentException("SSH key title already exists for this user", nameof(request));
      key.Title = request.Title;
    }

    if (!string.IsNullOrEmpty(request.PublicKey))
      key.PublicKey = request.PublicKey;

    await _repo.Update(key);
    return new SshKeyResponse(key);
  }

  public async Task Delete(int idToDelete, ClaimsPrincipal requester)
  {
    var key =
      await _repo.GetById(idToDelete) ?? throw new KeyNotFoundException("SSH key not found");

    // Check permissions
    if (requester.GetUserId() != key.UserId && !requester.IsAdmin())
      throw new UnauthorizedAccessException("You do not have permission to delete this SSH key");

    await _repo.Delete(key);
  }

  public async Task<SshKeyResponse> GetByBlob(string blob)
  {
    var sshKey = await _repo.GetByBlob(blob) ?? throw new KeyNotFoundException("SSH key not found");
    return new SshKeyResponse(sshKey);
  }
}
