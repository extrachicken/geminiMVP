using Godot;

/// A door trigger area. When the player presses E, transitions to TargetScene
/// and spawns the player at TargetSpawnPoint in the new room.
public partial class DoorInteraction : InteractableBase
{
    [Export] public string TargetScene      { get; set; } = "";
    [Export] public string TargetSpawnPoint { get; set; } = "DefaultSpawn";

    public override void _Ready()
    {
        base._Ready();
        // Label is created by the room setup; nothing extra needed here.
    }

    public override void Interact(CharacterBody2D player)
    {
        if (string.IsNullOrEmpty(TargetScene)) return;
        SceneTransitionManager.Instance.GoToScene(TargetScene, TargetSpawnPoint);
    }
}
