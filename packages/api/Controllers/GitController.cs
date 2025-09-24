using Api.Models.Requests;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/git")]
public class GitController : ControllerBase
{
  private readonly IGitService _gitService;

  public GitController(IGitService gitService)
  {
    _gitService = gitService;
  }

  [HttpPost("authorize")]
  public async Task<IActionResult> Authorize([FromBody] GitAuthorizationRequest request)
  {
    var isAuthorized = await _gitService.IsAuthorized(request);
    return isAuthorized ? Ok() : Forbid();
  }
}
