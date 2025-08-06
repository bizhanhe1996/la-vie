namespace LaVie.Structs;

public struct DeleteButtonStruct(string controller, int id)
{
    public string Controller = controller;
    public int Id = id;
    public readonly string Permission
    {
        get { return Controller.ToUpper() + "_DELETE"; }
    }
}
