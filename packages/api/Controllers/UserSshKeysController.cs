using Api.Models.Requests;
using Api.Models.Responses;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/users/{id}/ssh-keys")]
[Authorize]
public class UserSshKeysController : ControllerBase
{
  private readonly ISshKeyService _sshService;

  public UserSshKeysController(ISshKeyService sshService)
  {
    _sshService = sshService;
  }

  [HttpGet]
  public async Task<ActionResult<List<SshKeyResponse>>> GetUserSshKeys(int id)
  {
    try
    {
      var keys = await _sshService.GetByOwnerId(id, User);
      return Ok(keys);
    }
    catch (UnauthorizedAccessException)
    {
      return Forbid();
    }
    catch (KeyNotFoundException)
    {
      return NotFound();
    }
  }

  [HttpPost]
  public async Task<ActionResult<SshKeyResponse>> Create(int id, CreateSshKeyRequest request)
  {
    try
    {
      var created = await _sshService.Create(request, id, User);
      return CreatedAtAction(nameof(SshKeysController.GetById), new { id = created.Id }, created);
    }
    catch (UnauthorizedAccessException)
    {
      return Forbid();
    }
    catch (ArgumentException ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }
}
