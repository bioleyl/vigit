using Api.Extensions;
using Api.Repositories;
using Api.Repositories.Interfaces;
using Api.Services;
using Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddGitConfiguration(builder.Configuration);
builder.Services.AddControllers();

// Add repositories.
builder.Services.AddScoped<IGitRepository, GitRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRepositoryRepository, RepositoryRepository>();
builder.Services.AddScoped<ISshKeyRepository, SshKeyRepository>();

// Add application services.
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRepositoryService, RepositoryService>();
builder.Services.AddScoped<ISshKeyService, SshKeyService>();
builder.Services.AddScoped<IGitService, GitService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerWithJwt();

var app = builder.Build();

// Write Git SSH config files
app.Services.WriteGitConfigFiles();

// Initialize and seed the database
app.Services.SeedDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  app.UseCors(policy =>
  {
    policy.AllowAnyOrigin();
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
  });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "API is running!");

await app.RunAsync();

public partial class Program
{
  protected Program() { }
}
