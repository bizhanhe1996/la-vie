using System.ComponentModel.DataAnnotations;

namespace LaVie.Models;

public class Tag : BaseModel
{
    public Tag() { }

    public Tag(string name)
    {
        Name = name;
    }

    [Required]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    // relationships
    public ICollection<ProductTag>? ProductTags { get; set; }
}
