using Api.Data;
using Api.Services.Interfaces;
using Api.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Tests;

public abstract class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
  private bool _disposed;
  protected readonly HttpClient _client;
  private readonly IServiceScope _scope;

  protected IntegrationTestBase(CustomWebApplicationFactory factory)
  {
    _client = factory.CreateClient();

    // Add a JWT token for authentication (default to admin user)
    var jwtService = factory.Services.GetRequiredService<IJwtService>();
    _client.AddJwtToken(jwtService, "admin", "Admin");

    // Create a scope so we can work with DI services
    _scope = factory.Services.CreateScope();
    var db = _scope.ServiceProvider.GetRequiredService<AppDbContext>();

    ResetDatabase(db);
  }

  protected virtual void ResetDatabase(AppDbContext db)
  {
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    db.SaveChanges();

    Helpers.DbInitializer.Seed(db);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!_disposed)
    {
      if (disposing)
      {
        _scope.Dispose();
        _client.Dispose();
      }

      _disposed = true;
    }
  }

  public void Dispose()
  {
    Dispose(disposing: true);
    GC.SuppressFinalize(this);
  }
}
