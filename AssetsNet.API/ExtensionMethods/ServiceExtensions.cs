using System.Text;
using AssetsNet.API.Data;
using AssetsNet.API.Entities;
using AssetsNet.API.Helpers.Cloudinary;
using AssetsNet.API.Interfaces;
using AssetsNet.API.Interfaces.Auth;
using AssetsNet.API.Interfaces.ChatGpt;
using AssetsNet.API.Interfaces.Email;
using AssetsNet.API.Interfaces.News;
using AssetsNet.API.Interfaces.Photo;
using AssetsNet.API.Interfaces.Reddit;
using AssetsNet.API.Interfaces.Repositories;
using AssetsNet.API.Interfaces.Stock;
using AssetsNet.API.Interfaces.Twitter;
using AssetsNet.API.Models.Email;
using AssetsNet.API.Repositories.User;
using AssetsNet.API.Seed;
using AssetsNet.API.Seed.Models;
using AssetsNet.API.Services.Auth;
using AssetsNet.API.Services.ChatGtp;
using AssetsNet.API.Services.News;
using AssetsNet.API.Services.Photo;
using AssetsNet.API.Services.Reddit;
using AssetsNet.API.Services.Stocks;
using AssetsNet.API.Services.Twitter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

            opts.User.AllowedUserNameCharacters = null;
            opts.User.RequireUniqueEmail = true;

        }).AddEntityFrameworkStores<AssetsDbContext>().AddDefaultTokenProviders();
    }

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenHandler, Services.TokenHandler>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, Services.Email.EmailService>();
        services.AddScoped<IPhotoService, PhotoService>();

        services.AddScoped<IChatGptService, ChatGptService>();
        services.AddScoped<INewsService, NewsService>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<IRedditService, RedditService>();
        services.AddScoped<ITwitterService, TwitterService>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<SeedRolesService>();
        services.AddScoped<SeedAdminAccountService>();

        services.Configure<AdminAccountCredentials>(configuration.GetSection("AdminAccountCredentials"));
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<CloudinaryCredentials>(configuration.GetSection("CloudinarySettings"));
    }

    public static void ConfigureAuthentification(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(cfg =>
        {
            cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
    }
}