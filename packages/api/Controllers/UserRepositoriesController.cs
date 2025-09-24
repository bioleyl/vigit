using Api.Extensions;
using Api.Models.Requests;
using Api.Models.Responses;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/users/{id}/repositories")]
[Authorize]
public class UserRepositoriesController : ControllerBase
{
  private readonly IRepositoryService _repositoryService;

  public UserRepositoriesController(IRepositoryService repositoryService)
  {
    _repositoryService = repositoryService;
  }

  [HttpGet]
  public async Task<ActionResult<List<RepositoryResponse>>> GetUserRepositories(int id)
  {
    try
    {
      var repos = await _repositoryService.GetByOwnerId(id, User);
      return Ok(repos);
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
  public async Task<ActionResult<RepositoryResponse>> Create(
    int id,
    CreateRepositoryRequest request
  )
  {
    try
    {
      var created = await _repositoryService.Create(request, id, User);
      return CreatedAtRoute("GetRepositoryById", new { id = created.Id }, created);
    }
    catch (ArgumentException ex)
    {
      return BadRequest(new { message = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }
}
