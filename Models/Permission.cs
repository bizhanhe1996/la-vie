namespace LaVie.Models;

public class Permission(string title, string role) : BaseModel
{
    public string Title { get; set; } = title;
    public string Role { get; set; } = role;
}
