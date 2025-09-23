using Api.Services;
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
        options.TokenValidationParameters = JwtService.GetTokenValidationParameters(jwtSettings);
      });

    return services;
  }
}
