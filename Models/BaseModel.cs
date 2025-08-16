namespace LaVie.Models;

using LaVie.Extras.Interfaces;

/*
    User class must be updated manually
    because it does not implement IModel
*/
public class BaseModel : IModel
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; }
}
