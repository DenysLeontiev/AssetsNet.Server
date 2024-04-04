using System.Text.Json.Serialization;
using AssetsNet.API.ExtensionMethods;
using AssetsNet.API.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureAssetsDbContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.ConfigureAuthentification(builder.Configuration);
builder.Services.ConfigureAutoMapper();

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

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

var seedRolesService = services.GetRequiredService<SeedRolesService>();
var userSeedService = services.GetRequiredService<SeedAdminAccountService>();

await seedRolesService.SeedRolesAsync();
await userSeedService.SeedAdminUserAsync();

app.Run();
