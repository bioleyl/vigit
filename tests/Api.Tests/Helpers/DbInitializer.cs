using Api.Data;
using Api.Entities;

namespace Api.Tests.Helpers;

public static class DbInitializer
{
  public static void Seed(AppDbContext context)
  {
    if (context.Users.Any())
      return;

    context.Users.AddRange(
      new User
      {
        Username = "admin",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
        Role = "Admin",
      },
      new User
      {
        Username = "user",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("user"),
        Role = "User",
      }
    );

    context.SaveChanges();
  }
}
