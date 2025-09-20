using Api.Models.Requests;
using Api.Models.Responses;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/repositories")]
[Authorize]
public class RepositoriesController : ControllerBase
{
  private readonly IRepositoryService _repositoryService;

  public RepositoriesController(IRepositoryService repositoryService)
  {
    _repositoryService = repositoryService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<RepositoryWithCollaboratorsResponse>> GetById(int id)
  {
    try
    {
      var repo = await _repositoryService.GetById(id, User);
      return Ok(repo);
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
  public async Task<ActionResult<RepositoryResponse>> Update(
    int id,
    UpdateRepositoryRequest request
  )
  {
    try
    {
      var updated = await _repositoryService.Update(id, request, User);
      return updated == null ? NotFound() : Ok(updated);
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
      await _repositoryService.Delete(id, User);
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

  [HttpPost("{id}/collaborators/{userId}")]
  public async Task<IActionResult> AddCollaborator(int id, int userId)
  {
    try
    {
      await _repositoryService.AddCollaborator(id, userId, User);
      return NoContent();
    }
    catch (UnauthorizedAccessException)
    {
      return Forbid();
    }
    catch (KeyNotFoundException ex)
    {
      return NotFound(new { message = ex.Message });
    }
    catch (ArgumentException ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }

  [HttpDelete("{id}/collaborators/{userId}")]
  public async Task<IActionResult> RemoveCollaborator(int id, int userId)
  {
    try
    {
      await _repositoryService.RemoveCollaborator(id, userId, User);
      return NoContent();
    }
    catch (UnauthorizedAccessException)
    {
      return Forbid();
    }
    catch (KeyNotFoundException ex)
    {
      return NotFound(new { message = ex.Message });
    }
  }
}
