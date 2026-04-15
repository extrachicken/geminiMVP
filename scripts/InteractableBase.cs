using Godot;

/// Abstract base for all interactable objects (doors, items, NPCs).
/// Extends Area2D — place a CollisionShape2D child to define trigger zone.
/// Collision layer 3 = "interactables"; player body (layer 1) enters it.
public abstract partial class InteractableBase : Area2D
{
    /// Text shown in the [E] prompt when the player is near.
    [Export] public string InteractLabel { get; set; } = "Interact";

    private int _playersInside = 0; // count of player bodies overlapping

    public override void _Ready()
    {
        CollisionLayer = 4; // layer 3 (bitmask 4) = interactables
        CollisionMask  = 1; // detect player body (layer 1)
        BodyEntered += OnBodyEntered;
        BodyExited  += OnBodyExited;
    }

    /// Called when the player presses E while near this object.
    public abstract void Interact(CharacterBody2D player);

    /// Called when the player moves into range.
    protected virtual void OnPlayerEnter(CharacterBody2D player) { }

    /// Called when the player moves out of range.
    protected virtual void OnPlayerExit(CharacterBody2D player) { }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not CharacterBody2D player) return;
        _playersInside++;
        GameManager.SetNearestInteractable(this);
        OnPlayerEnter(player);
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is not CharacterBody2D player) return;
        _playersInside = Mathf.Max(0, _playersInside - 1);
        if (_playersInside == 0 && GameManager.NearestInteractable == this)
            GameManager.SetNearestInteractable(null);
        OnPlayerExit(player);
    }
}
