using AngularMarketplace.Server;
using Microsoft.EntityFrameworkCore;

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

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
