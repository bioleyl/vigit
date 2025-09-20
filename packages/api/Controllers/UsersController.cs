using Api.Models.Requests;
using Api.Models.Responses;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
  private readonly IUserService _userService;

  public UsersController(IUserService userService)
  {
    _userService = userService;
  }

  [HttpGet]
  public async Task<ActionResult<List<UserResponse>>> GetAll()
  {
    var users = await _userService.GetAll();
    return Ok(users);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<UserFullResponse>> GetById(int id)
  {
    try
    {
      var user = await _userService.GetById(id, User);
      return Ok(user);
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
  public async Task<ActionResult<UserResponse>> Create(CreateUserRequest request)
  {
    try
    {
      var created = await _userService.Create(request, User);
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
      var updated = await _userService.Update(id, request, User);
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
      await _userService.Delete(id, User);
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
