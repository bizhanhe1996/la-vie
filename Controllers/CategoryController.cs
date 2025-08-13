using Bogus;
using LaVie.Data;
using LaVie.Extras.Enums;
using LaVie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaVie.Controllers;

[Authorize]
public class CategoryController : BaseController
{
    private readonly MyAppContext context;

    public CategoryController(MyAppContext context, UserManager<User> userManager)
        : base("Category", "Categories", userManager)
    {
        this.context = context;
    }

    public async Task<IActionResult> Index(int page = 1, int size = 10)
    {
        // breadcrumnbs
        SetIndexBreadcrumbs();
        // pagination
        Paginator
            .SetTotalCount(await context.Categories.CountAsync())
            .SetPage(page)
            .SetSize(size)
            .Run();
        // database
        List<Category> categories = await context
            .Categories.OrderBy(c => c.Id)
            .Skip(Paginator.SkipCount)
            .Take(Paginator.TakeCount)
            .Include(c => c.Products)
            .ToListAsync();
        // result
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

    [HttpGet]
    public async Task<IActionResult> Seed()
    {
        var faker = new Faker<Category>()
            .RuleFor(
                c => c.Title,
                f => f.Random.AlphaNumeric(6).ToLower() + f.Commerce.Categories(1).First()
            )
            .RuleFor(c => c.Slug, (f, c) => MakeCategorySlug(c.Title))
            .RuleFor(c => c.Description, f => f.Lorem.Sentence(12));
        var fakeCategories = faker.Generate(100);

        context.Categories.AddRange(fakeCategories);
        await context.SaveChangesAsync();
        return RedirectToAction("Index");
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

    private string MakeCategorySlug(string title)
    {
        return title
            .ToLowerInvariant()
            .Replace(" ", "-")
            .Replace(",", "")
            .Replace(".", "")
            .Replace("/", "")
            .Replace("'", "");
    }
}
