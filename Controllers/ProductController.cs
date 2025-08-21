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
    public ProductController(MyAppContext context, UserManager<User> userManager)
        : base("Product", "Products", context, userManager) { }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int size = 10)
    {
        SetIndexBreadcrumbs();
        Paginator
            .SetTotalCount(await _context.Products.CountAsync())
            .SetPage(page)
            .SetSize(size)
            .Run();

        var products = await _context
            .Products.OrderBy(p => p.Id)
            .Skip(Paginator.SkipCount)
            .Take(Paginator.TakeCount)
            .Include(p => p.Category)
            .Include(p => p.ProductTags!)
            .ThenInclude(pt => pt.Tag)
            .ToListAsync();
        return View(products);
    }

    public async Task<IActionResult> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return RedirectToAction("Index");
        }
        query = query.ToUpper();

        var products = await _context
            .Products.Where(p => p.Name.ToUpper().Contains(query))
            .Include(p => p.Category)
            .Include(p => p.ProductTags!)
            .ThenInclude(pt => pt.Tag)
            .ToListAsync();

        SetIndexBreadcrumbs();
        return View("~/Views/Product/Index.cshtml", products);
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

            _context.Products.Add(product!);
            await _context.SaveChangesAsync();
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
        Product? updatable = await _context.Products.FindAsync(id);
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
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
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
        return await Delete<Product>(_context, id);
    }

    private ProductController SetCategories()
    {
        ViewBag.Categories = _context.Categories.ToList();
        return this;
    }

    private ProductController SetTags()
    {
        var Tags = new Dictionary<string, string>();
        _context.Tags.ToList().ForEach(t => Tags.Add(t.Name, t.Id.ToString()));
        ViewBag.Tags = Tags;
        return this;
    }

    private bool CheckUniqueConstraint(Product product)
    {
        bool isTaken = _context.Products.Any(p => p.Name == product.Name);
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
