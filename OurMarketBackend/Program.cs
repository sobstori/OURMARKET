using Microsoft.EntityFrameworkCore;
using OurMarketBackend.Data; 

var builder = WebApplication.CreateBuilder(args);

// Register ApplicationDbContext with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Enable serving static files (like CSS/JS)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Map routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
