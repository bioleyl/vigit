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
      new User(username: "admin", password: "admin", role: "Admin"),
      new User(username: "user", password: "user", role: "User")
    );

    context.SaveChanges();
  }
}
