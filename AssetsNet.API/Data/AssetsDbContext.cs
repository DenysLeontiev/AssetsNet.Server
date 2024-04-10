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

        builder.Entity<UserFollow>()
            .HasKey(uf => new { uf.UserId, uf.FollowerId });

        builder.Entity<UserFollow>()
            .HasOne(uf => uf.User)
            .WithMany(u => u.Followers)
            .HasForeignKey(uf => uf.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserFollow>()
            .HasOne(uf => uf.Follower)
            .WithMany(u => u.Following)
            .HasForeignKey(uf => uf.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

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
    public DbSet<UserFollow> UserFollows { get; set; }
}