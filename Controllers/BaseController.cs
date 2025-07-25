using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SampleProject.Data;
using SampleProject.Enums;
using SampleProject.Models;
using SampleProject.Structs;
using SampleProject.Utils;

namespace SampleProject.Controllers;

public class BaseController : Controller
{
    public BaseController(string controller, string indexTitle, UserManager<User> userManager)
    {
        this.Controller = controller;
        this.IndexTitle = indexTitle;
        this._userManager = userManager;
        this.Paginator = new(this);
    }

    private string Controller { set; get; }
    private string IndexTitle { set; get; }
    protected readonly UserManager<User> _userManager;
    protected readonly Paginator Paginator;

    protected Controller SetViewMode(VIEW_MODES viewMode)
    {
        ViewBag.ViewMode = viewMode;
        return this;
    }

    protected Controller SetIndexBreadcrumbs()
    {
        ViewBag.Path = new BreadcrumbItem[]
        {
            new()
            {
                Controller = Controller,
                Action = "Index",
                Title = IndexTitle,
            },
        };
        return this;
    }

    protected Controller SetCreateBreadcrumbs()
    {
        ViewBag.Path = new BreadcrumbItem[]
        {
            new()
            {
                Controller = Controller,
                Action = "Index",
                Title = IndexTitle,
            },
            new()
            {
                Controller = Controller,
                Action = "Create",
                Title = "Create",
            },
        };
        return this;
    }

    protected Controller SetUpdateBreadcrumbs()
    {
        ViewBag.Path = new BreadcrumbItem[]
        {
            new()
            {
                Controller = Controller,
                Action = "Index",
                Title = IndexTitle,
            },
            new()
            {
                Controller = Controller,
                Action = "Update",
                Title = "Update",
            },
        };
        return this;
    }

    protected void WriteModelErrors()
    {
        Console.WriteLine("\n\n");
        foreach (var state in ModelState)
        {
            var key = state.Key;
            foreach (var error in state.Value.Errors)
            {
                var errorMessage = error.ErrorMessage;
                Console.WriteLine($"Error in '{key}': {errorMessage}");
            }
        }
        Console.WriteLine("\n\n");
    }

    public async Task<IActionResult> Delete<Entity>(MyAppContext context, int id)
        where Entity : class
    {
        var dbSet = context.Set<Entity>();
        var deletable = await dbSet.FindAsync(id);
        if (deletable == null)
        {
            return NotFound();
        }
        else
        {
            dbSet.Remove(deletable);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        if (User.Identity.IsAuthenticated)
        {
            ViewBag.ActiveUser = _userManager.GetUserAsync(User).Result;
        }
    }
}
