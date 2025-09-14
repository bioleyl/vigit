using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class DbExtensions
{
  public static IServiceCollection AddDatabase(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    var connectionString =
      configuration.GetConnectionString("DefaultConnection")
      ?? throw new InvalidOperationException("DefaultConnection is not configured!");

    services.AddDbContext<AppDbContext>(options =>
      options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
    );

    return services;
  }

  public static void SeedDatabase(this IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Ensure database is created
    context.Database.EnsureCreated();

    // Call your DbInitializer
    DbInitializer.Initialize(context);
  }
}
