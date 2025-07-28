namespace LaVie.Seeders;

using LaVie.Interfaces;
using Microsoft.AspNetCore.Identity;

public class RolesSeeder: ISeeder {

    private readonly string[] roles = ["Admin", "User"];
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public RolesSeeder(RoleManager<IdentityRole<int>> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<int> Seed() {
        foreach (var role in roles) {
        if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole<int>(role));
            }
        }
        return 0;
    }

}