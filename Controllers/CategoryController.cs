using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleProject.Data;
using SampleProject.Enums;
using SampleProject.Models;
using SampleProject.Structs;

namespace SampleProject.Controllers;

[Authorize]
public class CategoryController : BaseController
{
    private readonly MyAppContext context;

    public CategoryController(MyAppContext context, UserManager<User> userManager)
        : base("Category", "Categories", userManager)
    {
        this.context = context;
    }

    public IActionResult Index()
    {
        SetIndexBreadcrumbs();
        List<Category> categories = context.Categories.Include(c => c.Products).ToList();
        return View(categories);
    }

    [HttpGet]
    public IActionResult Create()
    {
        SetViewMode(VIEW_MODES.CREATE);
        SetCreateBreadcrumbs();
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Update(int Id)
    {
        var category = await context.Categories.FindAsync(Id);
        if (category == null)
        {
            return NotFound();
        }
        else
        {
            SetViewMode(VIEW_MODES.UPDATE);
            SetUpdateBreadcrumbs();
            return View("Create", category);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Update(Category category)
    {
        if (ModelState.IsValid)
        {
            context.Categories.Update(category);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        else
        {
            WriteModelErrors();
            SetUpdateBreadcrumbs();
            SetViewMode(VIEW_MODES.UPDATE);
            return View("Create", category);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        if (ModelState.IsValid)
        {
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        else
        {
            SetCreateBreadcrumbs();
            return View(category);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        return await Delete<Category>(context, id);
    }

    private bool CheckUniqueConstraint(Category category)
    {
        bool isTaken = context.Categories.Any(c => c.Title == category.Title);
        if (isTaken)
        {
            ModelState.AddModelError("Name", "This title is already taken.");
        }
        return isTaken;
    }
}
