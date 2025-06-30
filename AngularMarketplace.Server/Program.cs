using AngularMarketplace.Server;
using AngularMarketplace.Server.Extensions;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Access to appsettings.json from Controllers

builder.Services.AddSingleton(builder.Configuration);

// Identity extension method
builder.Services
    .AddIdentityHandlersAndStored()
    .ConfigureIdentityOptions()
    .AddIdentityAuth(builder.Configuration);


// Db Context
builder.Services.AddDbContext<AppDbContext>(options=>
    options.UseSqlite(builder.Configuration.GetConnectionString("mydb"))
);

builder.Services.AddControllers();

// JSON Serializer
builder.Services.AddControllersWithViews().AddNewtonsoftJson();




var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// CORS

app
    .ConfigureCORS()
    .AddIdentityAuthMiddlwares();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Images")),
    RequestPath = "/cdn"
});



app.MapControllers();


app.MapFallbackToFile("/index.html");

app.Run();
