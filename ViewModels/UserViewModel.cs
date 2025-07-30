using System.ComponentModel.DataAnnotations;

namespace LaVie.ViewModels;

public class UserViewModel
{

    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, number, and special character.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string RawPassword { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password Repeat")]
    [Compare("RawPassword", ErrorMessage = "Passwords do not match.")]
    public string RawPasswordRepeat { get; set; } = null!;

    [Required]
    [RegularExpression("Admin|Manager|Client", ErrorMessage = "Role must be Admin, Manager, or Client.")]
    [Display(Name = "Role")]
    public string Role { get; set; } = null!;
    
}