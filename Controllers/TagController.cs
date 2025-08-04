using LaVie.Data;
using LaVie.Enums;
using LaVie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaVie.Controllers;

[Authorize]
public class TagController : BaseController
{
    private readonly MyAppContext context;

    public TagController(MyAppContext context, UserManager<User> userManager)
        : base("Tag", "Tags", userManager)
    {
        this.context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        SetIndexBreadcrumbs();
        List<Tag> tags = context.Tags.Include(tag => tag.ProductTags).ToList();
        return View(tags);
    }

    [HttpGet]
    public IActionResult Create()
    {
        SetCreateBreadcrumbs();
        SetViewMode(VIEW_MODES.CREATE);
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var tag = await context.Tags.FindAsync(id);
        if (tag == null)
        {
            return NotFound();
        }
        else
        {
            SetUpdateBreadcrumbs();
            SetViewMode(VIEW_MODES.UPDATE);
            return View("Create", tag);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Update(Tag tag)
    {
        if (ModelState.IsValid)
        {
            context.Tags.Update(tag);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        else
        {
            SetUpdateBreadcrumbs();
            SetViewMode(VIEW_MODES.UPDATE);
            return View("Create", tag);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(Tag tag)
    {
        if (ModelState.IsValid)
        {
            context.Tags.Add(tag);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        else
        {
            return View(tag);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        return await Delete<Tag>(context, id);
    }
}
