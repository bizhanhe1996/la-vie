using LaVie.Data;
using LaVie.Models;
using LaVie.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LaVie.Controllers;

public class ProfileController : BaseController
{
    private readonly IWebHostEnvironment _env;

    public ProfileController(
        UserManager<User> userManager,
        IWebHostEnvironment env,
        MyAppContext context
    )
        : base("Profile", "Profile", context, userManager)
    {
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        SetIndexBreadcrumbs();
        var user = await _userManager.GetUserAsync(User);
        var profileViewModel = new ProfileViewModel()
        {
            FirstName = user?.FirstName ?? "",
            LastName = user?.LastName ?? "",
            Email = user?.Email ?? "",
            AvatarPath = user?.AvatarPath,
        };

        return View("~/Views/Settings/Profile.cshtml", profileViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ProfileViewModel model)
    {
        SetIndexBreadcrumbs();

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return BadRequest();
        }
        user.Email = model.Email;

        if (model.AvatarFile != null && model.AvatarFile.Length > 0)
        {
            var uploadPath = Path.Combine(_env.WebRootPath, "uploads");

            if (Directory.Exists(uploadPath) == false)
                Directory.CreateDirectory(uploadPath);

            uploadPath = Path.Combine(uploadPath, "profiles");
            if (Directory.Exists(uploadPath) == false)
                Directory.CreateDirectory(uploadPath);

            // Create unique file name
            var fileName = Path.GetFileName(model.AvatarFile.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.AvatarFile.CopyToAsync(stream);
            }
            user.AvatarPath = filePath.Substring(filePath.IndexOf("/uploads"));
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}
