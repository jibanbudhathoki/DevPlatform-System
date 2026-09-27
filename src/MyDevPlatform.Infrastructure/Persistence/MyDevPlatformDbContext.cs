using Microsoft.EntityFrameworkCore;
using MyDevPlatform.Domain.Organizations;
using MyDevPlatform.Domain.Projects;
using MyDevPlatform.Domain.Subscriptions;
using MyDevPlatform.Domain.Users;

namespace MyDevPlatform.Infrastructure.Persistence;

public class MyDevPlatformDbContext : DbContext
{
    public MyDevPlatformDbContext(DbContextOptions<MyDevPlatformDbContext> options)
        : base(options)
    {
    }

    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<PlatformProject> PlatformProjects => Set<PlatformProject>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Slug).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(200).IsRequired();
            entity.Property(x => x.ApiKey).HasMaxLength(256);
            entity.Property(x => x.BaseApiUrl).HasMaxLength(500);
            entity.HasIndex(x => x.Slug).IsUnique();
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Status).IsRequired();
            entity.HasIndex(x => x.OrganizationId).IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(200).IsRequired();
            entity.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Slug).HasMaxLength(120).IsRequired();
            entity.Property(x => x.TemplateId).HasMaxLength(150).IsRequired();
            entity.HasIndex(x => new { x.OrganizationId, x.Slug }).IsUnique();
        });

        modelBuilder.Entity<PlatformProject>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Slug).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(2000);
            entity.Property(x => x.RootPath).HasMaxLength(500).IsRequired();
            entity.Property(x => x.TemplateId).HasMaxLength(150).IsRequired();
            entity.HasIndex(x => x.Slug).IsUnique();
        });
    }
}
