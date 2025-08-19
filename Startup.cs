using DotNetEnv;
using LaVie.Data;
using LaVie.Extras.Authorization.Handlers;
using LaVie.Extras.Authorization.Requirements;
using LaVie.Extras.EventManagers;
using LaVie.Extras.Filters;
using LaVie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace LaVie;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void AddServices(IServiceCollection services)
    {
        LoadEnvironmentVariables();
        AddControllersWithViews(services);
        AddSingletonServices(services);
        AddSQLiteService(services);
        AddIdentityService(services);
        ConfigureIdentity(services);
        AddPolicies(services);
    }

    private static void LoadEnvironmentVariables()
    {
        Env.Load();
    }

    private static void AddControllersWithViews(IServiceCollection services)
    {
        services.AddControllersWithViews(options =>
        {
            options.Filters.Add<GlobalsInjectorFilter>();
        });
    }

    private static void AddSingletonServices(IServiceCollection services)
    {
        services.AddSingleton<GlobalsInjectorFilter>();
        services.AddSingleton<IAuthorizationHandler, MaximumLoginHourHandler>();
        services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
        services.AddSingleton<AppEventManager>();
    }

    private static void ConfigureIdentity(IServiceCollection services)
    {
        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Lockout.AllowedForNewUsers = false;
        });
    }

    private static void AddPolicies(IServiceCollection services)
    {
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

    private static void AddIdentityService(IServiceCollection services)
    {
        services
            .AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<MyAppContext>()
            .AddDefaultTokenProviders();
    }

    public void UseMiddlewares(WebApplication app, IHostEnvironment env)
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

        if (app.Environment.IsDevelopment() == false)
        {
            app.UseHttpsRedirection();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
    }

    private void AddSQLiteService(IServiceCollection services)
    {
        services.AddDbContext<MyAppContext>(options =>
            options.UseSqlite(_configuration.GetConnectionString("DefaultConnection"))
        );
    }
}
