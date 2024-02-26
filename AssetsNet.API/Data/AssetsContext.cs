using Microsoft.EntityFrameworkCore;

namespace AssetsNet.API.Data;

public class AssetsContext : DbContext
{
    public AssetsContext(DbContextOptions<AssetsContext> options) : base(options)
    {
        
    }
}