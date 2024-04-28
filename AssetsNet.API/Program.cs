using System.Text.Json.Serialization;
using AssetsNet.API.ExtensionMethods;
using AssetsNet.API.Hubs;
using AssetsNet.API.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    // add JWT Authentication
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", // must be lower case
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] { }}
    });
});



// builder.Services.AddControllers().AddJsonOptions(x =>
//    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

builder.Services.ConfigureAssetsDbContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.ConfigureAuthentification(builder.Configuration);
builder.Services.ConfigureAutoMapper();

builder.Services.AddSignalR();

builder.Services.AddCors();

builder.Services.AddHttpClient();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(builder.Configuration.GetValue<string>("ClientUrl"));
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<MessageHub>("/hubs/message");

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

var seedRolesService = services.GetRequiredService<SeedRolesService>();
var userSeedService = services.GetRequiredService<SeedAdminAccountService>();

await seedRolesService.SeedRolesAsync();
await userSeedService.SeedAdminUserAsync();

app.Run();
