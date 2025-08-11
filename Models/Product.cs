using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LaVie.Interfaces;

namespace LaVie.Models;

public class Product : BaseModel
{
    public Product() { }

    public Product(string name, double price, int categoryId)
    {
        Name = name;
        Price = price;
        CategoryId = categoryId;
    }

    public int Id { set; get; }

    [Required]
    public string Name { set; get; } = null!;

    public double? Price { set; get; }

    [Required]
    public int CategoryId { set; get; }
    public Category? Category { set; get; }

    [NotMapped]
    public int[]? SelectedTagsIds { set; get; }

    public ICollection<ProductTag>? ProductTags { set; get; }
}
