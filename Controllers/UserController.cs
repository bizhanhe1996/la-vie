using LaVie.Data;
using LaVie.Extras.Enums;
using LaVie.Models;
using LaVie.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaVie.Controllers;

[Authorize(Roles = "Admin,Manager")]
public class UserController : BaseController
{
    protected readonly MyAppContext context;

    public UserController(MyAppContext context, UserManager<User> userManager)
        : base("User", "Users", userManager)
    {
        this.context = context;
    }

    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Index()
    {
        var users = await context.Users.ToListAsync();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            user.Role = roles.FirstOrDefault("Default");
        }

        SetIndexBreadcrumbs();
        return View(users);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        SetCreateBreadcrumbs();
        SetViewMode(VIEW_MODES.CREATE);
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(UserViewModel userViewModel)
    {
        if (ModelState.IsValid)
        {
            var user = new User(userViewModel.FirstName, userViewModel.LastName)
            {
                Email = userViewModel.Email,
            };

            var result = await _userManager.CreateAsync(user, userViewModel.RawPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, userViewModel.Role);
            }
            return RedirectToAction("Index");
        }
        else
        {
            SetCreateBreadcrumbs();
            SetViewMode(VIEW_MODES.CREATE);
            return View(userViewModel);
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int Id)
    {
        var user = await context.Users.FindAsync(Id);
        if (user == null)
        {
            return NotFound();
        }
        else
        {
            SetUpdateBreadcrumbs();
            SetViewMode(VIEW_MODES.UPDATE);
            return View("Create", user);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(User user)
    {
        if (ModelState.IsValid)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        else
        {
            SetUpdateBreadcrumbs();
            SetViewMode(VIEW_MODES.UPDATE);
            return View("Create", user);
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        return await Delete<User>(this.context, id);
    }
}
