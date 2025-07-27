using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaVie.Data;
using LaVie.Enums;
using LaVie.Models;

namespace LaVie.Controllers;

[Authorize(Roles = "Admin")]
public class UserController : BaseController
{
    protected readonly MyAppContext context;

    public UserController(MyAppContext context, UserManager<User> userManager)
        : base("User", "Users", userManager)
    {
        this.context = context;
    }

    public async Task<IActionResult> Index()
    {
        var users = await context.Users.ToListAsync();
        SetIndexBreadcrumbs();
        return View(users);
    }

    [HttpGet]
    public IActionResult Create()
    {
        SetCreateBreadcrumbs();
        SetViewMode(VIEW_MODES.CREATE);
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        if (ModelState.IsValid)
        {
            var newUser = context.Users.Add(user);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        else
        {
            SetCreateBreadcrumbs();
            SetViewMode(VIEW_MODES.CREATE);
            return View(user);
        }
    }

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

    public async Task<IActionResult> Delete(int id)
    {
        return await Delete<User>(this.context, id);
    }
}
