using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace LaVie.Models;

public class User : IdentityUser<int>
{
    // must be present to prevent error
    public User() { }

    public User(string firstName, string lastName)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
    }

    [PersonalData]
    public override string? UserName
    {
        get => Email;
        set { }
    }

    public override string? NormalizedUserName
    {
        get => NormalizedEmail;
        set { }
    }

    [Required]
    public string FirstName { set; get; } = null!;

    [Required]
    public string LastName { set; get; } = null!;

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
