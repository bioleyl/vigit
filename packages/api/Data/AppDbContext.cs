using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options) { }

  public DbSet<User> Users { get; set; }
  public DbSet<Repository> Repositories { get; set; }
  public DbSet<UserRepository> UserRepositories { get; set; }
  public DbSet<SshKey> SshKeys { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<UserRepository>().HasKey(ur => new { ur.UserId, ur.RepositoryId });

    modelBuilder
      .Entity<UserRepository>()
      .HasOne(ur => ur.User)
      .WithMany(u => u.Collaborations)
      .HasForeignKey(ur => ur.UserId);

    modelBuilder
      .Entity<UserRepository>()
      .HasOne(ur => ur.Repository)
      .WithMany(r => r.Collaborators)
      .HasForeignKey(ur => ur.RepositoryId);

    modelBuilder
      .Entity<SshKey>()
      .HasOne(k => k.User)
      .WithMany(u => u.SshKeys)
      .HasForeignKey(k => k.UserId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
