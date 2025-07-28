namespace LaVie.Models;
using LaVie.Interfaces;

public class Permission: IModel {

    public int Id {get;set;}

    public string Title {get;set;} = null!;

    public string Value { get; set; } = "true";

    public BaseModel? Module { get; set; } = null!; 

    public string? Description { get; set; }

}