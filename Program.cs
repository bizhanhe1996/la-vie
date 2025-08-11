// uses
using DotNetEnv;
using LaVie.Authorization.Handlers;
using LaVie.Authorization.Requirements;
using LaVie.Data;
using LaVie.Filters;
using LaVie.Models;
using LaVie.Seeders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddSingleton<IAuthorizationHandler, MaximumLoginHourHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();

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

// Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "AgeAndTimePolicy",
        policy =>
        {
            // policy.Requirements.Add(new MinimumAgeRequirement(18));
            policy.Requirements.Add(new MaximumLoginHourRequirement(24));
        }
    );
});

// building the application
var app = builder.Build();

// seeding claims and roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

    var identitySeeder = new IdentitySeeder(roleManager);
    await identitySeeder.Seed();
}

// configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() == false)
{
    app.UseExceptionHandler("/Home/Error");
    // the default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// middlewares are used here
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// routing
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

// running the application
app.Run();
