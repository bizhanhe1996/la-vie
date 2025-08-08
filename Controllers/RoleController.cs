using LaVie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LaVie.Controllers;

[Authorize(Roles = "Admin")]
public class RoleController : BaseController
{
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public RoleController(RoleManager<IdentityRole<int>> roleManager, UserManager<User> userManager)
        : base("Role", "Roles", userManager)
    {
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        SetIndexBreadcrumbs();

        var roles = _roleManager.Roles.ToList();
        var counts = new Dictionary<string, Dictionary<string, int>>();

        foreach (var role in roles)
        {
            var claims = await _roleManager.GetClaimsAsync(role);
            var users = await _userManager.GetUsersInRoleAsync(role.Name ?? "");
            counts.Add(
                role.Name ?? "",
                new Dictionary<string, int>()
                {
                    { "permissions", claims.Count },
                    { "users", users.Count },
                }
            );
        }
        ViewBag.Counts = counts;
        return View(roles);
    }

    public async Task<IActionResult> Update(int id)
    {
        SetUpdateBreadcrumbs();
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role == null)
        {
            return NotFound();
        }

        ViewBag.Permissions = await _roleManager.GetClaimsAsync(role);

        return View(role);
    }

    // public async Task<IActionResult> Update() { 

    //     return 2;
    // }
}
