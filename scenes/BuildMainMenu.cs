using Godot;

/// Builds scenes/main_menu.tscn:
///   Control (full-screen) with MainMenuUI script.
///
/// Run: dotnet build && timeout 60 godot --headless --script scenes/BuildMainMenu.cs
public partial class BuildMainMenu : SceneBuilderBase
{
    public override void _Initialize()
    {
        GD.Print("Building: main_menu.tscn");

        var temp = new Node();
        var root = new Control();
        root.Name = "MainMenu";
        root.SetAnchorsPreset(Control.LayoutPreset.FullRect);
        temp.AddChild(root);

        root.SetScript(GD.Load("res://scripts/MainMenuUI.cs"));

        var rootNode = temp.GetChild(0);
        temp.RemoveChild(rootNode);
        temp.Free();

        PackAndSave(rootNode, "res://scenes/main_menu.tscn");
    }
}
