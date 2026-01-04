using AdFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdFlow.Infrastructure.Data;

public class AdFlowDbContext : DbContext
{
    public AdFlowDbContext(DbContextOptions<AdFlowDbContext> options) : base(options)
    {
    }

    public DbSet<FacebookUser> FacebookUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FacebookUser>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FacebookId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ProfilePictureUrl).HasMaxLength(500);
            entity.HasIndex(e => e.FacebookId).IsUnique();
            entity.HasIndex(e => e.Email);
        });
    }
}
