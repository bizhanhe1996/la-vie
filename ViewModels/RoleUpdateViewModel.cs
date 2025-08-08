using System.ComponentModel.DataAnnotations;

namespace LaVie.ViewModels;

public class RoleUpdateViewModel
{
    [Required]
    public string Role { get; set; } = null!;

    [MinLength(1)]
    public string[] Permissions { get; set; } = null!;
}
