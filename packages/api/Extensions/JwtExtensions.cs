using Api.Services;
using Api.Services.Interfaces;
using Api.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Api.Extensions;

public static class JwtExtensions
{
  public static IServiceCollection AddJwtAuthentication(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    // Bind and validate settings
    var jwtSettings =
      configuration.GetSection("Jwt").Get<JwtSettings>()
      ?? throw new InvalidOperationException("JWT section missing");
    jwtSettings.Validate();

    // Registering settings for DI
    services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

    // Register JwtService in DI
    var jwtService = new JwtService(jwtSettings);
    services.AddSingleton<IJwtService>(jwtService);

    // Configure authentication with JWT Bearer
    services
      .AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(options =>
      {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = jwtService.GetTokenValidationParameters();
      });

    return services;
  }
}
