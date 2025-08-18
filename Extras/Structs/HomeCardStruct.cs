namespace LaVie.Extras.Structs;

public struct HomeCardStruct(string title, string message, string icon, string @class, int count)
{
    public string Title { get; set; } = title;
    public string Message { get; set; } = message;
    public string Icon { get; set; } = icon;
    public string Class { get; set; } = @class;
    public int Count { get; set; } = count;
}
