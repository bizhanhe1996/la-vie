using LaVie.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LaVie.Seeders;

public class PermissionsSeeder: ISeeder {


    public PermissionsSeeder(RoleManager<IdentityRole<int>> _roleManagerParameters)
    {
        
    }

    private readonly RoleManager<IdentityRole<int>> _roleManager;

    private readonly string[] claims = [
        "User.Create",
        "User.Index",
        "User.Update",
        "User.Delete",
        "Product.Create",
        "Product.Index",
        "Product.Update",
        "Product.Delete",
        "Category.Create",
        "Category.Index",
        "Category.Update",
        "Category.Delete",
        "Tag.Create",
        "Tag.Index",
        "Tag.Update",
        "Tag.Delete",
    ];


    public async Task<int> Seed() {
        return 0;
 

    }


}