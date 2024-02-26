using AssetsNet.API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssetsNet.API.Data;

public class AssetsDbContext : IdentityDbContext
{
    public AssetsDbContext(DbContextOptions<AssetsDbContext> options) : base(options)
    {
        
    }
}