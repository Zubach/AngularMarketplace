using AngularMarketplace.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.UseCors(x=> x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());



// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Images")),
    RequestPath = "/cdn"
});

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
