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
public class UsersController : ControllerBase
{
  private readonly IUserService _userService;
  private readonly IRepositoryService _repositoryService;

  public UsersController(IUserService userService, IRepositoryService repositoryService)
  {
    _userService = userService;
    _repositoryService = repositoryService;
  }

  [HttpGet]
  public async Task<ActionResult<List<UserResponse>>> GetAll()
  {
    var users = await _userService.GetAll();
    return Ok(users);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<UserResponse>> GetById(int id)
  {
    var user = await _userService.GetById(id);
    return user == null ? NotFound() : Ok(user);
  }

  [HttpGet("{id}/repositories")]
  public async Task<ActionResult<List<RepositoryResponse>>> GetUserRepositories(int id)
  {
    var user = await _userService.GetById(id);
    if (user == null)
      return NotFound();

    var repos = await _repositoryService.GetByOwnerId(id);
    return Ok(repos);
  }

  [HttpPost]
  public async Task<ActionResult<UserResponse>> Create(CreateUserRequest request)
  {
    try
    {
      var created = await _userService.Create(request, User.IsAdmin());
      return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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

  [HttpPut("{id}")]
  public async Task<ActionResult<UserResponse>> Update(int id, UpdateUserRequest request)
  {
    try
    {
      var updated = await _userService.Update(id, request, User.GetUserId(), User.IsAdmin());
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
      await _userService.Delete(id, User.GetUserId(), User.IsAdmin());
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
}
