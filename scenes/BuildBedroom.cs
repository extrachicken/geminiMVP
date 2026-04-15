using Godot;

/// Builds scenes/bedroom.tscn
public partial class BuildBedroom : SceneBuilderBase
{
    public override void _Initialize()
    {
        GD.Print("Building: bedroom.tscn");

        var temp = new Node();
        var root = new Node2D();
        root.Name = "Bedroom";
        temp.AddChild(root);

        root.SetScript(GD.Load("res://scripts/BedroomScene.cs"));

        var rootNode = temp.GetChild(0);
        temp.RemoveChild(rootNode);
        temp.Free();

        PackAndSave(rootNode, "res://scenes/bedroom.tscn");
    }
}
