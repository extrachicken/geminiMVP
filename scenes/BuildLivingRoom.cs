using Godot;

/// Builds scenes/living_room.tscn
public partial class BuildLivingRoom : SceneBuilderBase
{
    public override void _Initialize()
    {
        GD.Print("Building: living_room.tscn");

        var temp = new Node();
        var root = new Node2D();
        root.Name = "LivingRoom";
        temp.AddChild(root);

        root.SetScript(GD.Load("res://scripts/LivingRoomScene.cs"));

        var rootNode = temp.GetChild(0);
        temp.RemoveChild(rootNode);
        temp.Free();

        PackAndSave(rootNode, "res://scenes/living_room.tscn");
    }
}
