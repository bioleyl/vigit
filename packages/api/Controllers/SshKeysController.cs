using Api.Models.Requests;
using Api.Models.Responses;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/ssh-keys")]
[Authorize]
public class SshKeysController : ControllerBase
{
  private readonly ISshKeyService _sshService;

  public SshKeysController(ISshKeyService sshService)
  {
    _sshService = sshService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<SshKeyResponse>> GetById(int id)
  {
    try
    {
      var key = await _sshService.GetById(id, User);
      return Ok(key);
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

  [HttpPut("{id}")]
  public async Task<ActionResult<SshKeyResponse>> Update(int id, UpdateSshKeyRequest request)
  {
    try
    {
      var updated = await _sshService.Update(id, request, User);
      return Ok(updated);
    }
    catch (UnauthorizedAccessException)
    {
      return Forbid();
    }
    catch (KeyNotFoundException)
    {
      return NotFound();
    }
    catch (ArgumentException ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    try
    {
      await _sshService.Delete(id, User);
      return NoContent();
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

  // This method is public, even though the controller is protected
  [HttpGet("lookup/{escapedBlob}")]
  [AllowAnonymous]
  public async Task<IActionResult> Lookup(string escapedBlob)
  {
    var blob = escapedBlob;
    if (string.IsNullOrEmpty(blob))
      return BadRequest();

    blob = Uri.UnescapeDataString(blob);

    try
    {
      var key = await _sshService.GetByBlob(blob);
      return Content(key.PublicKey);
    }
    catch (KeyNotFoundException)
    {
      return NotFound();
    }
  }
}
