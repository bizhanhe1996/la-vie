using System.Security.Claims;
using LaVie.Data;
using LaVie.Enums;
using LaVie.Filters;
using LaVie.Models;
using LaVie.Structs;
using LaVie.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LaVie.Controllers;

[ServiceFilter(typeof(GlobalsInjectorFilter))]
public class BaseController : Controller, IActionFilter
{
    public BaseController(string controller, string indexTitle, UserManager<User> userManager)
    {
        Controller = controller;
        IndexTitle = indexTitle;
        _userManager = userManager;
        Paginator = new(this);
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
        ViewBag.Path = new Breadcrumb[]
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
        ViewBag.Path = new Breadcrumb[]
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
        ViewBag.Path = new Breadcrumb[]
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
            SetActiveUserRole(context);
        }
    }

    private void SetActiveUserRole(ActionExecutingContext context)
    {
        var roles = context
            .HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        ViewBag.ActiveUserRole = roles[0];
    }
}
