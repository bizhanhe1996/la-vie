namespace LaVie.Models;

using LaVie.Interfaces;

public class Permission(string title, string role) : IModel
{
    public string Title { get; set; } = title;
    public string Role { get; set; } = role;
}
