using Godot;

/// Builds scenes/player.tscn:
///   CharacterBody2D (PlayerController)
///   ├── CollisionShape2D   (CapsuleShape2D)
///   ├── Sprite2D           (hero spritesheet)
///   ├── Camera2D           (smooth follow)
///   └── InteractionArea (Area2D)
///       └── CollisionShape2D (CircleShape2D r=70)
///
/// Run: dotnet build && timeout 60 godot --headless --script scenes/BuildPlayer.cs
public partial class BuildPlayer : SceneBuilderBase
{
    public override void _Initialize()
    {
        GD.Print("Building: player.tscn");

        var temp = new Node();
        var root = new CharacterBody2D();
        root.Name = "Player";
        root.CollisionLayer = 1; // "player"
        root.CollisionMask  = 2; // collide with "world"
        temp.AddChild(root);

        // ── Capsule collision ─────────────────────────────────────────────────
        var colShape = new CollisionShape2D();
        var capsule  = new CapsuleShape2D();
        capsule.Height = 50f;
        capsule.Radius = 22f; // total height = 50 + 2*22 = 94px
        colShape.Shape = capsule;
        colShape.Position = new Vector2(0, -47f); // center of capsule = 47px above origin
        root.AddChild(colShape);

        // ── Sprite2D ──────────────────────────────────────────────────────────
        var sprite = new Sprite2D();
        sprite.Name = "Sprite2D";
        // Texture loaded at runtime by PlayerController; builder sets up scale/offset defaults.
        // We try to load it here too, so the scene file stores the reference.
        var tex = GD.Load<Texture2D>("res://sprites/hero_spritesheet.png");
        if (tex != null)
        {
            sprite.Texture = tex;
            int fw = tex.GetWidth() / 3; // one character view width
            sprite.RegionEnabled = true;
            sprite.RegionRect = new Rect2(0, 0, fw, tex.GetHeight()); // front view default
            float scale = 160f / tex.GetHeight();
            sprite.Scale = new Vector2(scale, scale);
            // Shift sprite so feet align with capsule bottom (origin at capsule center → top offset)
            sprite.Offset = new Vector2(0, -tex.GetHeight() / 2f + (50f + 22f));
        }
        root.AddChild(sprite);

        // ── Camera2D ──────────────────────────────────────────────────────────
        var camera = new Camera2D();
        camera.Name = "Camera2D";
        camera.PositionSmoothingEnabled = true;
        camera.PositionSmoothingSpeed   = 6f;
        camera.Zoom = new Vector2(1f, 1f);
        root.AddChild(camera);

        // ── Interaction Area ──────────────────────────────────────────────────
        var interactArea = new Area2D();
        interactArea.Name        = "InteractionArea";
        interactArea.CollisionLayer = 0;
        interactArea.CollisionMask  = 4; // detect interactables (layer 3, bitmask=4)
        var interactShape = new CollisionShape2D();
        var interactCircle = new CircleShape2D();
        interactCircle.Radius = 72f;
        interactShape.Shape = interactCircle;
        interactShape.Position = new Vector2(0, -47f); // centered on player torso
        interactArea.AddChild(interactShape);
        root.AddChild(interactArea);

        // ── Script (last — SetScript disposes wrapper, use temp parent) ───────
        root.SetScript(GD.Load("res://scripts/PlayerController.cs"));

        var rootNode = temp.GetChild(0);
        temp.RemoveChild(rootNode);
        temp.Free();

        PackAndSave(rootNode, "res://scenes/player.tscn");
    }
}
