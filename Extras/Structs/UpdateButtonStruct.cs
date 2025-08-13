namespace LaVie.Extras.Structs;

public struct UpdateButtonStruct(string controller, int id)
{
    public string Controller = controller;
    public int Id = id;

    public string Permission
    {
        get { return Controller.ToUpper() + "_UPDATE"; }
    }
}
