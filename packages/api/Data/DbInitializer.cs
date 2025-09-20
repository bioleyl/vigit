using Api.Entities;

namespace Api.Data;

public static class DbInitializer
{
  public static void Initialize(AppDbContext context)
  {
    context.Database.EnsureCreated();

    if (context.Users.Any())
      return; // already seeded

    context.Users.AddRange(
      new User(username: "admin", password: "admin", role: "Admin"),
      new User(username: "user", password: "user", role: "User")
    );
    context.SaveChanges();
  }
}
