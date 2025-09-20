using Api.Extensions;
using Api.Models.Requests;
using Api.Models.Responses;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    var repo = await _repositoryService.GetById(id);
    return repo == null ? NotFound() : Ok(repo);
  }

  [HttpGet("my")]
  public async Task<ActionResult<List<RepositoryWithCollaboratorsResponse>>> GetMyRepositories()
  {
    var repos = await _repositoryService.GetByOwnerId(User.GetUserId());
    return Ok(repos);
  }

  [HttpPost]
  public async Task<ActionResult<RepositoryResponse>> Create(CreateRepositoryRequest request)
  {
    try
    {
      var created = await _repositoryService.Create(request, User.GetUserId());
      return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
    catch (ArgumentException ex)
    {
      return BadRequest(new { message = ex.Message });
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
      var updated = await _repositoryService.Update(id, request, User.GetUserId(), User.IsAdmin());
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
      await _repositoryService.Delete(id, User.GetUserId(), User.IsAdmin());
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
      await _repositoryService.AddCollaborator(id, userId, User.GetUserId(), User.IsAdmin());
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
      await _repositoryService.RemoveCollaborator(id, userId, User.GetUserId(), User.IsAdmin());
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
