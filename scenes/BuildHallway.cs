using Godot;

/// Builds scenes/hallway.tscn (Node2D root with HallwayScene script).
/// All room geometry, items, and doors are created at runtime by HallwayScene._Ready().
///
/// Run: dotnet build && timeout 60 godot --headless --script scenes/BuildHallway.cs
public partial class BuildHallway : SceneBuilderBase
{
    public override void _Initialize()
    {
        GD.Print("Building: hallway.tscn");

        var temp = new Node();
        var root = new Node2D();
        root.Name = "Hallway";
        temp.AddChild(root);

        root.SetScript(GD.Load("res://scripts/HallwayScene.cs"));

        var rootNode = temp.GetChild(0);
        temp.RemoveChild(rootNode);
        temp.Free();

        PackAndSave(rootNode, "res://scenes/hallway.tscn");
    }
}
