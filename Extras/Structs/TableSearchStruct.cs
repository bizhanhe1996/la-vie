namespace LaVie.Extras.Structs;

public struct TableSearchStruct
{
    public string Controller { get; set; }
    public string TableId { get; set; }

    public TableSearchStruct(string controller, string tableId)
    {
        Controller = controller;
        TableId = tableId;
    }
}
