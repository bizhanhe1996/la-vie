// uses
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LaVie.Data;
using LaVie.Models;
using LaVie.Filters;

WebApplicationOptions options = new() { WebRootPath = "public" };

// building a builder
var builder = WebApplication.CreateBuilder(options);

// Environment variables
Env.Load();

// adding services
builder.Services.AddControllersWithViews(options =>
{
    // Filters
    options.Filters.Add<GlobalsInjectorFilter>();
});

// DI Container
builder.Services.AddSingleton<GlobalsInjectorFilter>();

// SQLite
builder.Services.AddDbContext<MyAppContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Identity
builder
    .Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<MyAppContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Lockout.AllowedForNewUsers = false;
});

// building the application
var app = builder.Build();

// seeding Roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    string[] roles = ["Admin" ,"User"];
    foreach (var role in roles){
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<int>(role));
        }
    }
}

// configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() == false)
{
    app.UseExceptionHandler("/Home/Error");
    // the default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// middlewares are used here
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// routing
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

// running the application
app.Run();


  

