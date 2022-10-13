using WeddingPlanner.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSession();  

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddDbContext<WPContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();    

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();


// Run these packages in terminal to install Entity Framework
// dotnet add package Pomelo.EntityFrameworkCore.MySql --version 6.0.1
// dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.3
