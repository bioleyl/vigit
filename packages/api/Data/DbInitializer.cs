using Api.Entities;

namespace Api.Data;

public static class DbInitializer
{
  public static void Initialize(AppDbContext context)
  {
    context.Database.EnsureCreated();

    if (context.Users.Any())
      return; // already seeded

    var admin = new User { Username = "admin", Role = "Admin" };
    admin.SetPassword("admin");

    var user = new User { Username = "user", Role = "User" };
    user.SetPassword("user");

    context.Users.AddRange(admin, user);

    context.SaveChanges();
  }
}
