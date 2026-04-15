using Godot;

/// Builds scenes/kitchen.tscn
public partial class BuildKitchen : SceneBuilderBase
{
    public override void _Initialize()
    {
        GD.Print("Building: kitchen.tscn");

        var temp = new Node();
        var root = new Node2D();
        root.Name = "Kitchen";
        temp.AddChild(root);

        root.SetScript(GD.Load("res://scripts/KitchenScene.cs"));

        var rootNode = temp.GetChild(0);
        temp.RemoveChild(rootNode);
        temp.Free();

        PackAndSave(rootNode, "res://scenes/kitchen.tscn");
    }
}
