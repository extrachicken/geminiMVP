using Godot;

/// Builds scenes/orb.tscn:
///   Node2D (OrbFollower)
///
/// Run: dotnet build && timeout 60 godot --headless --script scenes/BuildOrb.cs
public partial class BuildOrb : SceneBuilderBase
{
    public override void _Initialize()
    {
        GD.Print("Building: orb.tscn");

        var temp = new Node();
        var root = new Node2D();
        root.Name = "Orb";
        temp.AddChild(root);

        // Script (OrbFollower handles all visuals via _Draw() and _Process())
        root.SetScript(GD.Load("res://scripts/OrbFollower.cs"));

        var rootNode = temp.GetChild(0);
        temp.RemoveChild(rootNode);
        temp.Free();

        PackAndSave(rootNode, "res://scenes/orb.tscn");
    }
}
