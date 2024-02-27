using AssetsNet.API.Data;
using AssetsNet.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AssetsNet.API.ExtensionMethods;

public static class ServiceExtensions
{
    public static void ConfigureAssetsDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AssetsDbContext>(opts => opts.UseSqlServer(connectionString));
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>(opts =>
        {
            opts.Password.RequireDigit = false;
            opts.Password.RequireLowercase = false;
            opts.Password.RequireNonAlphanumeric = false;
            opts.Password.RequireUppercase = false;
            opts.Password.RequiredLength = 4;

            opts.User.RequireUniqueEmail = true;

        }).AddEntityFrameworkStores<AssetsDbContext>().AddDefaultTokenProviders();
    }
}