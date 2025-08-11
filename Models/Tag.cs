using System.ComponentModel.DataAnnotations;
using LaVie.Interfaces;

namespace LaVie.Models;

public class Tag : BaseModel
{
    public Tag() { }

    public Tag(string name)
    {
        Name = name;
    }

    // columns

    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    // relationships
    public ICollection<ProductTag>? ProductTags { get; set; }
}
