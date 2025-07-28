using System.ComponentModel.DataAnnotations;
using LaVie.Interfaces;

namespace LaVie.Models;

public class Category : IModel
{
    public int Id { set; get; }

    [Required]
    public string Title { set; get; } = null!;

    public string? Slug { set; get; }

    public string? Description { set; get; }

    // relationships
    public List<Product> Products { set; get; } = new List<Product>();

    public int ProductsCount => Products?.Count ?? 0;
}
