using AssetsNet.API.ExtensionMethods;
using AssetsNet.API.Interfaces.News;
using AssetsNet.API.Interfaces;
using AssetsNet.API.Seed;
using AssetsNet.API.Services.News;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureAssetsDbContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.ConfigureAuthentification(builder.Configuration);

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
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

var seedRolesService = services.GetRequiredService<SeedRolesService>();
var userSeedService = services.GetRequiredService<SeedAdminAccountService>();

await seedRolesService.SeedRolesAsync();
await userSeedService.SeedAdminUserAsync();

app.Run();
