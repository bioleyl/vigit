using Api.Data;
using Api.Entities;
using Api.Models.Requests;
using Api.Models.Responses;
using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class UserService : IUserService
{
  private readonly AppDbContext _context;

  public UserService(AppDbContext context)
  {
    _context = context;
  }

  public async Task<List<UserResponse>> GetAllAsync()
  {
    return await _context.Users.Select(u => u.ToResponse()).ToListAsync();
  }

  public async Task<UserResponse?> GetByIdAsync(int id)
  {
    var user = await _context.Users.FindAsync(id);
    return user?.ToResponse();
  }

  public async Task<UserResponse> CreateAsync(CreateUserRequest request)
  {
    var user = new User(request);

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return user.ToResponse();
  }

  public async Task<UserResponse?> UpdateAsync(int id, UpdateUserRequest request)
  {
    var user = await _context.Users.FindAsync(id);
    if (user == null)
      return null;

    user.Update(request);
    await _context.SaveChangesAsync();

    return user.ToResponse();
  }

  public async Task<bool> DeleteAsync(int id)
  {
    var user = await _context.Users.FindAsync(id);
    if (user == null)
      return false;

    _context.Users.Remove(user);
    await _context.SaveChangesAsync();
    return true;
  }
}
