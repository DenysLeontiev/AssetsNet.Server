using AssetsNet.API.Data;
using AssetsNet.API.Entities;
using AssetsNet.API.Helpers;
using AssetsNet.API.Seed.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AssetsNet.API.Seed;

public class SeedAdminAccountService
{
    private readonly AssetsDbContext _dbContext;
    private readonly AdminAccountCredentials _adminCreds;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public SeedAdminAccountService(IOptions<AdminAccountCredentials> options, 
        UserManager<User> userManager,
        AssetsDbContext dbContext,
        RoleManager<IdentityRole> roleManager)
    {
        _adminCreds = options.Value;
        _userManager = userManager;
        _dbContext = dbContext;
        _roleManager = roleManager;
    }

    public async Task InitializeContextAsync()
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

    public async Task SeedAdminUserAsync()
    {
        if (await _userManager.FindByNameAsync(_adminCreds.UserName) is not null)
        {
            return;
        }

        User amdinUserToCreate = new()
        {
            UserName = _adminCreds.UserName,
            Email = _adminCreds.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(amdinUserToCreate, _adminCreds.Password);

        if (result.Succeeded)
        {
            List<string> adminRoles = new List<string> { DbRolesConsts.MemberRole, DbRolesConsts.AdminRole };

            await _userManager.AddToRolesAsync(amdinUserToCreate, adminRoles);
        }
    }
}