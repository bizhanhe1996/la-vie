using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LaVie.Models;
using LaVie.ViewModels;
using LaVie.Filters;

namespace LaVie.Controllers;

[ServiceFilter(typeof(GlobalsInjectorFilter))]
public class AccountController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public IActionResult Register()
    {
        SetEnvironemtVariables();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new User
        {
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        SetEnvironemtVariables();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        returnUrl ??= Url.Content("/");

        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    private void WriteModelErrors()
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

    private void SetEnvironemtVariables()
    {
        ViewData["ENV_PROJECT_NAME"] = Environment.GetEnvironmentVariable("ENV_PROJECT_NAME");
    }
}
