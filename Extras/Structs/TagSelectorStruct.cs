namespace LaVie.Extras.Structs;

public struct TagSelectorStruct
{
    public TagSelectorStruct(Dictionary<string, string> pairs, string aspFor)
    {
        Pairs = pairs;
        AspFor = aspFor;
    }

    public Dictionary<string, string> Pairs { get; set; }
    public string AspFor { get; set; }
}
