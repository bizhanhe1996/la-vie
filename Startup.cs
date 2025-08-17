using DotNetEnv;
using LaVie.Data;
using LaVie.Extras.Authorization.Handlers;
using LaVie.Extras.Authorization.Requirements;
using LaVie.Extras.EventManagers;
using LaVie.Extras.Filters;
using LaVie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LaVie;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // Register services here
    public void ConfigureServices(IServiceCollection services)
    {
        Env.Load();

        services.AddControllersWithViews(options =>
        {
            options.Filters.Add<GlobalsInjectorFilter>();
        });

        services.AddSingleton<GlobalsInjectorFilter>();
        services.AddSingleton<IAuthorizationHandler, MaximumLoginHourHandler>();
        services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
        services.AddSingleton<AppEventManager>();

        // SQLite
        services.AddDbContext<MyAppContext>(options =>
            options.UseSqlite(_configuration.GetConnectionString("DefaultConnection"))
        );

        // Identity
        services
            .AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<MyAppContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Lockout.AllowedForNewUsers = false;
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                "AgeAndTimePolicy",
                policy =>
                {
                    policy.Requirements.Add(new MaximumLoginHourRequirement(22));
                }
            );
        });
    }

    public void ConfigureHttpRequestPipeline(WebApplication app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
    }
}
