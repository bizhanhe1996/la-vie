using System.Security.Claims;
using LaVie.Data;
using LaVie.Extras.Structs;
using LaVie.Models;
using LaVie.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LaVie.Controllers;

[Authorize(Roles = "Admin")]
public class RoleController : BaseController
{
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public RoleController(
        RoleManager<IdentityRole<int>> roleManager,
        UserManager<User> userManager,
        MyAppContext context
    )
        : base("Role", "Roles", context, userManager)
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

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        SetUpdateBreadcrumbs();
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role == null)
        {
            return NotFound();
        }
        ViewBag.Permissions = await _roleManager.GetClaimsAsync(role);
        ViewBag.RoleName = role.Name;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Update(RoleUpdateViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Find role by name
            var role = await _roleManager.FindByNameAsync(model.Role);
            if (role == null)
            {
                return NotFound("Role not found");
            }

            var existingClaims = await _roleManager.GetClaimsAsync(role);
            await RemoveClaimsOfRole(role, existingClaims);
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            await AddPermissionToRole(role, model.Permissions);
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            return RedirectToAction("Index");
        }
        else
        {
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Permissions(string name)
    {
        SetIndexBreadcrumbs(
            new Breadcrumb()
            {
                Controller = "Role",
                Action = "Permissions",
                Title = $"Permissions of {name}",
            }
        );

        var role = await _roleManager.FindByNameAsync(name);
        if (role == null)
        {
            return NotFound();
        }

        var claims = await _roleManager.GetClaimsAsync(role);
        List<string> permissions = claims.Select(c => c.Value).ToList();
        return View(permissions);
    }

    private async Task RemoveClaimsOfRole(IdentityRole<int> role, IList<Claim> claims)
    {
        foreach (var claim in claims)
        {
            var result = await _roleManager.RemoveClaimAsync(role, claim);
            if (result.Succeeded == false)
            {
                ModelState.AddModelError("", $"Failed to remove permission {claim.Value}");
            }
        }
    }

    private async Task AddPermissionToRole(IdentityRole<int> role, string[] permissions)
    {
        foreach (var permission in permissions)
        {
            var permissionClaim = new Claim("Permission", permission);
            var result = await _roleManager.AddClaimAsync(role, permissionClaim);
            if (result.Succeeded == false)
            {
                ModelState.AddModelError("", $"Failed to add permission {permission}");
            }
        }
    }
}
