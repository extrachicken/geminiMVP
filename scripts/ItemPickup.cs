using Godot;

/// A pickable item in the world. When the player interacts, it is added to inventory
/// and the node is removed from the scene.
public partial class ItemPickup : InteractableBase
{
    public ItemData? Item { get; set; }

    // Visual: a colored rectangle drawn with _Draw()
    private Color _drawColor = new Color(0.3f, 0.8f, 0.3f);
    private float _drawRadius = 14f;
    private bool _pickedUp = false;

    /// Set Item before adding to the tree.
    public void SetItem(ItemData item)
    {
        Item = item;
        _drawColor = item.IconColor;
        InteractLabel = $"Pick up  {item.Name}";
    }

    public override void _Ready()
    {
        base._Ready();
        SetNotifyLocalTransform(true);
    }

    public override void Interact(CharacterBody2D player)
    {
        if (_pickedUp || Item == null) return;
        bool added = InventoryManager.Instance.AddItem(Item);
        if (added)
        {
            _pickedUp = true;
            if (GameManager.NearestInteractable == this)
                GameManager.SetNearestInteractable(null);
            QueueFree();
        }
    }

    public override void _Draw()
    {
        // Glowing item circle
        DrawCircle(Vector2.Zero, _drawRadius + 4f, new Color(_drawColor.R, _drawColor.G, _drawColor.B, 0.25f));
        DrawCircle(Vector2.Zero, _drawRadius, _drawColor);
        // Small highlight dot
        DrawCircle(new Vector2(-_drawRadius * 0.3f, -_drawRadius * 0.3f), _drawRadius * 0.2f, new Color(1, 1, 1, 0.6f));
    }
}
