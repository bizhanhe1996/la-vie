using LaVie.Data;
using LaVie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LaVie.Controllers;

[Authorize(Roles = "Admin")]
public class PermissionController : BaseController
{
    private readonly MyAppContext _context;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public PermissionController(
        MyAppContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole<int>> roleManager
    )
        : base("Permission", "Permissions", userManager)
    {
        _roleManager = roleManager;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        SetIndexBreadcrumbs();
        var roles = _roleManager.Roles.ToList();
        var claimsOfRoles = new List<Permission>();
        foreach (var role in roles)
        {
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            claimsOfRoles.AddRange(
                roleClaims.Select(c => new Permission(c.Value, role.Name ?? "")).ToList()
            );
        }

        return View(claimsOfRoles);
    }
}
