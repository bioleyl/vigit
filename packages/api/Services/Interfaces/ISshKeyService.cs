using System.Security.Claims;
using Api.Models.Requests;
using Api.Models.Responses;

namespace Api.Services.Interfaces;

public interface ISshKeyService
{
  public Task<SshKeyResponse> GetById(int id, ClaimsPrincipal requester);
  public Task<List<SshKeyResponse>> GetByOwnerId(int ownerId, ClaimsPrincipal requester);
  public Task<SshKeyResponse> GetByBlob(string blob);
  public Task<SshKeyResponse> Create(
    CreateSshKeyRequest request,
    int ownerId,
    ClaimsPrincipal requester
  );
  public Task<SshKeyResponse> Update(
    int idToUpdate,
    UpdateSshKeyRequest request,
    ClaimsPrincipal requester
  );
  public Task Delete(int idToDelete, ClaimsPrincipal requester);
}
