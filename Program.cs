using LaVie.Extras.EventHandlers;
using LaVie.Extras.EventManagers;
using LaVie.Extras.Seeders;
using Microsoft.AspNetCore.Identity;

namespace LaVie;

public class Program
{
    public static async Task Main(string[] args)
    {
        var app = await CreateHostAsync(args);
        app.Run();
    }

    private static async Task<WebApplication> CreateHostAsync(string[] args)
    {
        WebApplicationOptions options = new() { WebRootPath = "public" };
        var builder = WebApplication.CreateBuilder(options);

        var startup = new Startup(builder.Configuration);
        startup.ConfigureServices(builder.Services);

        var app = builder.Build();

        startup.ConfigureHttpRequestPipeline(app, app.Environment);

        var eventManager = app.Services.GetRequiredService<AppEventManager>();
        var userHandler = new UserRegistryHandler();

        eventManager.UserRegistered += userHandler.OnUserRegistered;

        // seeding claims and roles
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<
                RoleManager<IdentityRole<int>>
            >();

            var identitySeeder = new IdentitySeeder(roleManager);
            await identitySeeder.Seed();
        }

        return app;
    }
}
