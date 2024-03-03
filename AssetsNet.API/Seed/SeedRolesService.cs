using AssetsNet.API.Data;
using AssetsNet.API.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AssetsNet.API.Seed;

public class SeedRolesService
{
    private readonly AssetsDbContext _dbContext;
    private readonly RoleManager<IdentityRole> _roleManager;

    public SeedRolesService(
    AssetsDbContext dbContext,
    RoleManager<IdentityRole> roleManager)
    {
        _dbContext = dbContext;
        _roleManager = roleManager;
    }

    public async Task SeedRolesAsync()
    {
        if (_dbContext.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Count() > 0)
        {
            await _dbContext.Database.MigrateAsync();
        }

        if (!await _roleManager.Roles.AnyAsync())
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = DbRolesConsts.AdminRole });
            await _roleManager.CreateAsync(new IdentityRole { Name = DbRolesConsts.MemberRole });
        }
    }
}