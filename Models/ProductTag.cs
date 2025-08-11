using System.ComponentModel.DataAnnotations;
using LaVie.Interfaces;

namespace LaVie.Models;

public class ProductTag : BaseModel
{
    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    [Required]
    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
