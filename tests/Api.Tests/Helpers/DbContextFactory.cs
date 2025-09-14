using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Tests.Helpers;

public static class DbContextFactory
{
  public static AppDbContext CreateInMemoryContext()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(Guid.NewGuid().ToString())
      .Options;

    var context = new AppDbContext(options);

    DbInitializer.Seed(context);

    return context;
  }
}
