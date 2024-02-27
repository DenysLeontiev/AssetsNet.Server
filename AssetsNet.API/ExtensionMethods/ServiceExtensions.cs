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
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AssetsDbContext>().AddDefaultTokenProviders();
    }
}