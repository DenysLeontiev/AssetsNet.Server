using AssetsNet.API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssetsNet.API.Data;

public class AssetsDbContext : IdentityDbContext<User>
{
    public AssetsDbContext(DbContextOptions<AssetsDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .HasOne(u => u.ProfilePhoto)
            .WithOne(p => p.User)
            .HasForeignKey<Photo>(p => p.UserId);

        builder.Entity<Photo>()
            .HasOne(p => p.User)
            .WithOne(u => u.ProfilePhoto)
            .HasForeignKey<Photo>(p => p.UserId);
    }

    public DbSet<Photo> Photos { get; set; }
}