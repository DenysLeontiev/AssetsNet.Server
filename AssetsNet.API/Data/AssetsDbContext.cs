using AssetsNet.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetsNet.API.Data;

public class AssetsDbContext : DbContext
{
    public AssetsDbContext(DbContextOptions<AssetsDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
}