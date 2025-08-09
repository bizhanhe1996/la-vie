using System.ComponentModel.DataAnnotations;

namespace LaVie.ViewModels;

public class ProfileViewModel
{
    [EmailAddress, Required]
    public string Email { set; get; } = null!;

    [Required]
    public string FirstName { set; get; } = null!;

    [Required]
    public string LastName { set; get; } = null!;

    public IFormFile AvatarFile { set; get; } = null!;
}
