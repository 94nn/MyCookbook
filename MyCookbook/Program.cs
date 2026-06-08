using Microsoft.EntityFrameworkCore;
using MyCookbook.API.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Register DbContext with SQL Server
builder.Services.AddDbContext<CookbookDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // ← Scalar UI at /scalar
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();