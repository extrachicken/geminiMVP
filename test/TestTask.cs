using Godot;

/// Captures all four rooms + main menu for visual verification.
public partial class TestTask : SceneTree
{
    private int  _frame = 0;
    private int  _stage = 0;

    private static readonly string[] Scenes = {
        "res://scenes/main_menu.tscn",
        "res://scenes/hallway.tscn",
        "res://scenes/kitchen.tscn",
        "res://scenes/bedroom.tscn",
        "res://scenes/living_room.tscn",
    };

    public override void _Initialize()
    {
        ChangeSceneToFile(Scenes[0]);
    }

    public override bool _Process(double delta)
    {
        _frame++;

        // Every 8 frames advance to next scene
        if (_frame % 8 == 0 && _stage < Scenes.Length - 1)
        {
            _stage++;
            ChangeSceneToFile(Scenes[_stage]);
        }

        if (_frame >= Scenes.Length * 8 + 4)
            Quit();

        return false;
    }
}
