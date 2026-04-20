using Godot;

/// Player character — CharacterBody2D.
/// Handles WASD movement, gravity, sprite animation, and interaction dispatch.
public partial class PlayerController : CharacterBody2D
{
    [Export] public float WalkSpeed = 210f;
    [Export] public float RunSpeed  = 350f;
    [Export] public float Gravity   = 900f;

    // Sprite sheet layout: 3 views side by side (front, side-right, back).
    // Each view occupies exactly 1/3 of the image width.
    private Sprite2D    _sprite      = null!;
    private Camera2D    _camera      = null!;
    private Area2D      _interactArea = null!;

    private int  _spriteWidth;   // full texture width
    private int  _spriteHeight;  // full texture height
    private int  _frameWidth;    // width of one character view

    private bool _facingRight = true;
    private bool _initialized = false; // camera lerp guard (see quirks.md)

    public override void _Ready()
    {
        AddToGroup("player");

        _sprite       = GetNode<Sprite2D>("Sprite2D");
        _camera       = GetNode<Camera2D>("Camera2D");
        _interactArea = GetNode<Area2D>("InteractionArea");

        // Measure sprite sheet and set quality / scale for 1080p
        if (_sprite.Texture != null)
        {
            _spriteWidth  = _sprite.Texture.GetWidth();
            _spriteHeight = _sprite.Texture.GetHeight();
            _frameWidth   = _spriteWidth / 3;

            _sprite.TextureFilter = CanvasItem.TextureFilterEnum.LinearWithMipmaps;
            float scale = 240f / _spriteHeight; // ~240px tall at 1080p
            _sprite.Scale = new Vector2(scale, scale);
            _sprite.Offset = new Vector2(0, -_spriteHeight / 2f + 72f); // feet at origin

            _sprite.RegionEnabled = true;
            SetIdlePose(); // start with front view
        }

        // Snap camera to player on first frame (prevents lerp swoop from origin)
        _camera.GlobalPosition = GlobalPosition;
        _initialized = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;
        var vel = Velocity;

        // Gravity
        if (!IsOnFloor())
            vel.Y += Gravity * dt;
        else if (vel.Y > 0)
            vel.Y = 0f;

        // Block movement while dialogue is open
        if (DialogueManager.Instance.IsShowing)
        {
            vel.X = Mathf.MoveToward(vel.X, 0f, WalkSpeed * 6f * dt);
            Velocity = vel;
            MoveAndSlide();
            UpdateSpriteAnimation(0f);
            return;
        }

        // Horizontal input
        float dir = Input.GetAxis("move_left", "move_right");
        float speed = Input.IsActionPressed("run") ? RunSpeed : WalkSpeed;
        float targetX = dir * speed;

        // Smooth acceleration / deceleration
        vel.X = Mathf.MoveToward(vel.X, targetX, speed * 8f * dt);

        Velocity = vel;
        MoveAndSlide();

        UpdateSpriteAnimation(dir);
        HandleInventoryInput();
        HandleInteractInput();
    }

    // ─── Sprite Animation ─────────────────────────────────────────────────────

    private void SetIdlePose()
    {
        // Front view = first column (x=0)
        _sprite.RegionRect = new Rect2(0, 0, _frameWidth, _spriteHeight);
        _sprite.FlipH = false;
    }

    private void SetWalkPose(bool movingRight)
    {
        // Side view = second column (x = frameWidth)
        _sprite.RegionRect = new Rect2(_frameWidth, 0, _frameWidth, _spriteHeight);
        _sprite.FlipH = !movingRight; // flip for left movement
    }

    private void UpdateSpriteAnimation(float direction)
    {
        if (Mathf.Abs(direction) > 0.05f)
        {
            bool right = direction > 0f;
            if (_facingRight != right)
                _facingRight = right;
            SetWalkPose(_facingRight);
        }
        else
        {
            SetIdlePose();
        }
    }

    // ─── Input ────────────────────────────────────────────────────────────────

    private void HandleInventoryInput()
    {
        if (Input.IsActionJustPressed("inventory_next")) InventoryManager.Instance.SelectNext();
        if (Input.IsActionJustPressed("inventory_prev")) InventoryManager.Instance.SelectPrev();
        if (Input.IsActionJustPressed("slot_1")) InventoryManager.Instance.SelectSlot(0);
        if (Input.IsActionJustPressed("slot_2")) InventoryManager.Instance.SelectSlot(1);
        if (Input.IsActionJustPressed("slot_3")) InventoryManager.Instance.SelectSlot(2);
        if (Input.IsActionJustPressed("slot_4")) InventoryManager.Instance.SelectSlot(3);
        if (Input.IsActionJustPressed("slot_5")) InventoryManager.Instance.SelectSlot(4);
        if (Input.IsActionJustPressed("slot_6")) InventoryManager.Instance.SelectSlot(5);
    }

    private void HandleInteractInput()
    {
        if (!Input.IsActionJustPressed("interact")) return;
        if (DialogueManager.Instance.IsShowing) return; // DialogueManager handles E itself

        var interactable = GameManager.NearestInteractable;
        interactable?.Interact(this);
    }
}
