namespace LaVie.Seeders;

using System.Security.Claims;
using LaVie.Interfaces;
using Microsoft.AspNetCore.Identity;

public class IdentitySeeder : ISeeder
{
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private static readonly string[] roles = ["Admin", "Manager", "Client"];
    private static readonly Dictionary<string, List<Claim>> rolesAndClaims = new()
    {
        {
            "Admin",
            [
                new("Permission", "Home.Index"),
                new("Permission", "User.Create"),
                new("Permission", "User.Index"),
                new("Permission", "User.Update"),
                new("Permission", "User.Delete"),
            ]
        },
        { "Manager", [new("Permission", "Home.Index"), new("Permission", "User.Index")] },
        { "Client", [new("Permission", "Home.Index")] },
    };

    public IdentitySeeder(RoleManager<IdentityRole<int>> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task Seed()
    {
        // foreach role
        foreach (var role in roles)
        {
            await CreateRoleIfNotExists(role);

            var roleEntity = await _roleManager.FindByNameAsync(role);
            var currentClaims = await _roleManager.GetClaimsAsync(roleEntity);

            if (rolesAndClaims.TryGetValue(role, out var claims))
            {
                foreach (var claim in claims)
                {
                    if (DoesClaimExist(currentClaims, claim) == false)
                    {
                        await _roleManager.AddClaimAsync(roleEntity, claim);
                    }
                }
            }
        }
    }

    private bool DoesClaimExist(IList<Claim> currentClaims, Claim claim)
    {
        return currentClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value);
    }

    private async Task CreateRoleIfNotExists(string role)
    {
        bool roleNotExists = await _roleManager.RoleExistsAsync(role) == false;
        if (roleNotExists)
        {
            var identityRole = new IdentityRole<int>(role);
            await _roleManager.CreateAsync(identityRole);
        }
    }
}
