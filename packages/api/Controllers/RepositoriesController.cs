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
  private readonly IRepositoryService _service;

  public RepositoriesController(IRepositoryService service)
  {
    _service = service;
  }

  [HttpGet("{id}")]
  public ActionResult<RepositoryWithCollaboratorsResponse> GetById(int id)
  {
    var repo = _service.GetById(id);
    return repo == null ? NotFound() : Ok(repo);
  }

  [HttpGet("my")]
  public ActionResult<List<RepositoryWithCollaboratorsResponse>> GetMyRepositories()
  {
    var repos = _service.GetByOwnerId(User.GetUserId());
    return Ok(repos);
  }

  [HttpPost]
  public ActionResult<RepositoryResponse> Create(CreateRepositoryRequest request)
  {
    try
    {
      var created = _service.Create(request, User.GetUserId());
      return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
    catch (ArgumentException ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }

  [HttpPut("{id}")]
  public ActionResult<RepositoryResponse> Update(int id, UpdateRepositoryRequest request)
  {
    try
    {
      var updated = _service.Update(id, request, User.GetUserId(), User.IsAdmin());
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
  public IActionResult Delete(int id)
  {
    try
    {
      _service.Delete(id, User.GetUserId(), User.IsAdmin());
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
  public IActionResult AddCollaborator(int id, int userId)
  {
    try
    {
      _service.AddCollaborator(id, userId, User.GetUserId(), User.IsAdmin());
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
  public IActionResult RemoveCollaborator(int id, int userId)
  {
    try
    {
      _service.RemoveCollaborator(id, userId, User.GetUserId(), User.IsAdmin());
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
