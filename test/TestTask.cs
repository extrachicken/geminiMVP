using Godot;

/// Captures main menu, hallway, kitchen, bedroom, living room for QA.
public partial class TestTask : SceneTree
{
    private int _frame = 0;
    private int _stage = 0;

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
        if (_frame % 10 == 0 && _stage < Scenes.Length - 1)
        {
            _stage++;
            ChangeSceneToFile(Scenes[_stage]);
        }
        if (_frame >= Scenes.Length * 10 + 5)
            Quit();
        return false;
    }
}
