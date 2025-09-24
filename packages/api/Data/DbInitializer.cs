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

    var sshKeyEd25519 = new SshKey(
      userId: normalUser.Id,
      title: "initial-ed25519-key",
      publicKey: "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIJDJxKIFunk9SQy2dpoNK0xULA2EvX/t7MwORerG51Qc myemail@example.com"
    );

    var sshKeyRsa = new SshKey(
      userId: normalUser.Id,
      title: "initial-rsa-key",
      publicKey: "ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAACAQDLabxoGoExjoDn5b2g39uAIgH4soUMCPHcWhOZLohmo6Kopuvs6We9hNobJq0klHWewpzLXP6vYVsQzXBYqB2uHH+grRZW4houA5kejZW+ZuDUBdPHhtiZJw9XSDlVb+wy7DjgKxbBbhiyrVhPdNBv0Op73W8NFMUtmXBa5JaOOK+7lR2JbvapzL6EDzPF96tScvZZJOlu3C2qEgARFm01v/qsO2YAUfRVoxgWypP7SyqU9gTc/HotcZVwQbBYO7rOhkR6KQxZxeC7RSdapsetMlTGcOOaxTyGXHwE4ngcjnBjd5hXba4hbzIIK9kTLvu+jp1qlz1oKOMndW7o8Q1A702eSvgs2HUDrQlv+PX5M7TL5nVzuuJeb0Lmxl+KkSShMAaN4uJQ2Uqa9nLMB47fgGcoTM8cybqMUx32ydiVmb0c6uw43KvUxB0k+WonEeI9yS54djsKNzkgaveabgkAyOlTH908wWUBAieqT/DLObj/iyNBtLIiFXeROD4rnYefI5GbrLUflQqw4b0I3Z/i8Czm4ag0FhKOkzVJ8Q4SdlyJyr82uNbQLQlYkiqzeqmuBcewGO7/Qhr21ZBRKZYhLAe/nWSJQa81TMCyLhrOC+HPeLOq+uBqQXZMpnsrim9Ad1+f5SzFWUjakyI+gywmJaCNxN1MrfZChpwAzAI18w== leal@yourdomain.com"
    );

    context.SshKeys.Add(sshKeyEd25519);
    context.SshKeys.Add(sshKeyRsa);

    context.SaveChanges();
  }
}
