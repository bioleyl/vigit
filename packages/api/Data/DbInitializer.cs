using Api.Entities;

namespace Api.Data;

public static class DbInitializer
{
  public static void Initialize(AppDbContext context)
  {
    context.Database.EnsureCreated();

    if (context.Users.Any())
      return; // already seeded

    var adminUser = new User(username: "admin", password: "admin", role: "Admin");
    var normalUser = new User(username: "user", password: "user", role: "User");

    context.Users.AddRange(adminUser, normalUser);
    context.SaveChanges();

    var repo = new Repository(
      name: "example-repo",
      ownerId: normalUser.Id,
      description: "Sample repository for seeding"
    );

    context.Repositories.Add(repo);
    context.SaveChanges();

    var sshKey = new SshKey(
      userId: normalUser.Id,
      title: "initial-key",
      publicKey: "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIJDJxKIFunk9SQy2dpoNK0xULA2EvX/t7MwORerG51Qc myemail@example.com"
    );

    context.SshKeys.Add(sshKey);

    context.SaveChanges();
  }
}
