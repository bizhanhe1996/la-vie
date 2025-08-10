namespace LaVie.Seeders;

using System.Runtime.CompilerServices;
using System.Security.Claims;
using LaVie.Enums;
using LaVie.Interfaces;
using Microsoft.AspNetCore.Identity;

public class IdentitySeeder(RoleManager<IdentityRole<int>> roleManager) : ISeeder
{
    private readonly RoleManager<IdentityRole<int>> _roleManager = roleManager;
    private static readonly string[] roles = ["Admin", "Manager", "Client"];
    private static readonly Dictionary<string, List<Claim>> rolesAndClaims = new()
    {
        {
            "Admin",
            [
                new("Permission", PERMISSIONS.HOME_INDEX.ToString()),
                new("Permission", PERMISSIONS.USER_CREATE.ToString()),
                new("Permission", PERMISSIONS.USER_INDEX.ToString()),
                new("Permission", PERMISSIONS.USER_UPDATE.ToString()),
                new("Permission", PERMISSIONS.USER_DELETE.ToString()),
                new("Permission", PERMISSIONS.ROLE_UPDATE.ToString()),
                new("Permission", PERMISSIONS.CATEGORY_INDEX.ToString()),
                new("Permission", PERMISSIONS.CATEGORY_CREATE.ToString()),
                new("Permission", PERMISSIONS.CATEGORY_DELETE.ToString()),
                new("Permission", PERMISSIONS.CATEGORY_UPDATE.ToString()),
            ]
        },
        {
            "Manager",
            [
                new("Permission", PERMISSIONS.HOME_INDEX.ToString()),
                new("Permission", PERMISSIONS.USER_INDEX.ToString()),
            ]
        },
        { "Client", [new("Permission", PERMISSIONS.HOME_INDEX.ToString())] },
    };

    public async Task Seed()
    {
        // foreach role
        foreach (var role in roles)
        {
            await CreateRoleIfNotExists(role);

            var roleEntity = await _roleManager.FindByNameAsync(role);
            if (roleEntity == null)
            {
                return;
            }
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
