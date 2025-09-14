using Api.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Tests.Helpers;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
  protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
      // Remove existing DbContext registration
      var descriptor = services.SingleOrDefault(d =>
        d.ServiceType == typeof(DbContextOptions<AppDbContext>)
      );
      if (descriptor != null)
        services.Remove(descriptor);

      // Use a unique in-memory database per test run
      var dbName = Guid.NewGuid().ToString();
      services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(dbName));
    });
  }
}
