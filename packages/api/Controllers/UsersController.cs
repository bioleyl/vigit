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
  public async Task<ActionResult<List<UserResponse>>> GetAll()
  {
    return Ok(await _userService.GetAllAsync());
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<UserResponse>> GetById(int id)
  {
    var user = await _userService.GetByIdAsync(id);
    return user == null ? NotFound() : Ok(user);
  }

  [HttpPost]
  public async Task<ActionResult<UserResponse>> Create(CreateUserRequest request)
  {
    var created = await _userService.CreateAsync(request);
    return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<UserResponse>> Update(int id, UpdateUserRequest request)
  {
    var updated = await _userService.UpdateAsync(id, request);
    return updated == null ? NotFound() : Ok(updated);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var deleted = await _userService.DeleteAsync(id);
    return deleted ? NoContent() : NotFound();
  }
}
