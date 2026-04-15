using Godot;

/// The AI companion orb — a glowing yellow sphere that floats near the player.
/// Drawn procedurally; emits soft light via PointLight2D.
public partial class OrbFollower : Node2D
{
    [Export] public float FollowSpeed    = 4f;
    [Export] public float HoverAmplitude = 6f;
    [Export] public float HoverSpeed     = 2f;
    [Export] public Vector2 Offset       = new Vector2(60f, -80f); // relative to player

    private CharacterBody2D? _player;
    private PointLight2D     _light = null!;
    private float            _time   = 0f;
    private bool             _initialized = false;

    // Orb visual colors
    private static readonly Color GlowOuter = new Color(1.0f, 0.85f, 0.1f, 0.35f);
    private static readonly Color GlowMid   = new Color(1.0f, 0.92f, 0.3f, 0.8f);
    private static readonly Color Core      = new Color(1.0f, 1.0f, 0.75f, 1.0f);

    public override void _Ready()
    {
        _light = new PointLight2D();
        _light.Color = new Color(1f, 0.88f, 0.3f);
        _light.Energy = 0.8f;
        _light.TextureScale = 2.5f;
        // Use default texture (bright center, falloff)
        AddChild(_light);
    }

    public override void _Process(double delta)
    {
        _time += (float)delta;

        // Find player
        if (_player == null || !IsInstanceValid(_player))
        {
            _player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
            if (_player == null) return;
        }

        Vector2 target = _player.GlobalPosition + Offset;
        // Hover oscillation
        target.Y += Mathf.Sin(_time * HoverSpeed) * HoverAmplitude;

        if (!_initialized)
        {
            GlobalPosition = target;
            _initialized = true;
        }
        else
        {
            GlobalPosition = GlobalPosition.Lerp(target, FollowSpeed * (float)delta);
        }

        QueueRedraw();
    }

    public override void _Draw()
    {
        // Outer glow
        DrawCircle(Vector2.Zero, 20f, GlowOuter);
        // Mid glow
        DrawCircle(Vector2.Zero, 12f, GlowMid);
        // Core
        DrawCircle(Vector2.Zero, 7f, Core);
        // Specular highlight
        DrawCircle(new Vector2(-3f, -3f), 2.5f, new Color(1, 1, 1, 0.9f));
    }
}
