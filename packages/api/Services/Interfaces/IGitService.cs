using Api.Models.Requests;

namespace Api.Services.Interfaces;

public interface IGitService
{
  public Task<bool> IsAuthorized(GitAuthorizationRequest request);
}
