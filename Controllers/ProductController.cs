using LaVie.Data;
using LaVie.Extras.Enums;
using LaVie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaVie.Controllers;

[Authorize]
public class ProductController : BaseController
{
    private readonly MyAppContext context;

    public ProductController(MyAppContext context, UserManager<User> userManager)
        : base("Product", "Products", userManager)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        SetIndexBreadcrumbs();
        var products = await context
            .Products.Include(p => p.Category)
            .Include(p => p.ProductTags)
            .ThenInclude(pt => pt.Tag)
            .ToListAsync();
        return View(products);
    }

    [HttpGet]
    public IActionResult Create()
    {
        SetCreateBreadcrumbs();
        SetViewMode(VIEW_MODES.CREATE);
        return SetCategories().SetTags().View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        if (CheckUniqueConstraint(product))
        {
            SetCreateBreadcrumbs();
            return SetCategories().SetTags().View(product);
        }

        if (ModelState.IsValid)
        {
            if (product == null)
            {
                SetCreateBreadcrumbs();
                return SetCategories().SetTags().View(product);
            }

            product.ProductTags = MapSelectedTagsIds(product?.SelectedTagsIds ?? []);

            context.Products.Add(product);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        else
        {
            SetCreateBreadcrumbs();
            return SetCategories().SetTags().View(product);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        Product? updatable = await context.Products.FindAsync(id);
        if (updatable == null)
        {
            return NotFound();
        }
        else
        {
            SetUpdateBreadcrumbs();
            SetViewMode(VIEW_MODES.UPDATE);
            return SetCategories().SetTags().View("Create", updatable);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Update(Product product)
    {
        if (ModelState.IsValid)
        {
            context.Products.Update(product);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        else
        {
            SetUpdateBreadcrumbs();
            SetViewMode(VIEW_MODES.UPDATE);
            return SetCategories().SetTags().View("Create", product);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        return await Delete<Product>(context, id);
    }

    private ProductController SetCategories()
    {
        ViewBag.Categories = context.Categories.ToList();
        return this;
    }

    private ProductController SetTags()
    {
        var Tags = new Dictionary<string, string>();
        context.Tags.ToList().ForEach(t => Tags.Add(t.Name, t.Id.ToString()));
        ViewBag.Tags = Tags;
        return this;
    }

    private bool CheckUniqueConstraint(Product product)
    {
        bool isTaken = context.Products.Any(p => p.Name == product.Name);
        if (isTaken)
        {
            ModelState.AddModelError("Name", "This name is already taken.");
        }
        return isTaken;
    }

    private List<ProductTag>? MapSelectedTagsIds(int[] ids)
    {
        if (ids == null || ids.Length == 0)
        {
            return null;
        }
        return ids.Select(item => new ProductTag { TagId = item }).ToList();
    }
}
