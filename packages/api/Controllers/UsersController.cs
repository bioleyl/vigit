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

  public UsersController(IUserService userService)
  {
    _userService = userService;
  }

  [HttpGet]
  public ActionResult<List<UserResponse>> GetAll()
  {
    var users = _userService.GetAll();
    return Ok(users);
  }

  [HttpGet("{id}")]
  public ActionResult<UserResponse> GetById(int id)
  {
    var user = _userService.GetById(id);
    return user == null ? NotFound() : Ok(user);
  }

  [HttpPost]
  public ActionResult<UserResponse> Create(CreateUserRequest request)
  {
    try
    {
      var created = _userService.Create(request, User.IsAdmin());
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
  public ActionResult<UserResponse> Update(int id, UpdateUserRequest request)
  {
    try
    {
      var updated = _userService.Update(id, request, User.GetUserId(), User.IsAdmin());
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
      _userService.Delete(id, User.GetUserId(), User.IsAdmin());
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
