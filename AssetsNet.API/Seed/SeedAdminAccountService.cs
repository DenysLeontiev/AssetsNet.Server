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
    private readonly AdminAccountCredentials _adminCreds;
    private readonly UserManager<User> _userManager;

    public SeedAdminAccountService(IOptions<AdminAccountCredentials> options, 
        UserManager<User> userManager)
    {
        _adminCreds = options.Value;
        _userManager = userManager;
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