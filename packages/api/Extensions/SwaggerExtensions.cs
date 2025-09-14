using Microsoft.OpenApi.Models;

namespace Api.Extensions;

public static class SwaggerExtensions
{
  public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
  {
    services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

      // JWT support
      c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
          Name = "Authorization",
          Type = SecuritySchemeType.Http,
          Scheme = "bearer",
          BearerFormat = "JWT",
          In = ParameterLocation.Header,
          Description = "Enter your valid token.\nExample: \"eyJhbGciOi...\"",
        }
      );

      c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer",
              },
            },
            Array.Empty<string>()
          },
        }
      );
    });

    return services;
  }
}
