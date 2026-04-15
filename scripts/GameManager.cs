using Godot;

/// Autoload singleton — global game state.
/// Tracks spawn points, the nearest interactable, and the interact prompt UI.
public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; } = null!;

    /// The spawn point name that the next loaded room scene will position the player at.
    public static string TargetSpawnPoint = "DefaultSpawn";

    /// Currently focused interactable (set by InteractableBase when player enters its area).
    public static InteractableBase? NearestInteractable { get; private set; }

    // Interact prompt UI elements (CanvasLayer overlay)
    private CanvasLayer _promptLayer = null!;
    private Label _promptLabel = null!;

    [Signal]
    public delegate void NearestInteractableChangedEventHandler(bool hasInteractable);

    public override void _Ready()
    {
        Instance = this;
        BuildPromptUI();
    }

    public void SetPromptVisible(bool visible) => _promptLayer.Visible = visible;

    public static void SetNearestInteractable(InteractableBase? ia)
    {
        NearestInteractable = ia;
        if (Instance._promptLabel == null) return;

        if (ia != null)
        {
            Instance._promptLabel.Text = $"[E]  {ia.InteractLabel}";
            Instance._promptLabel.Visible = true;
        }
        else
        {
            Instance._promptLabel.Visible = false;
        }

        Instance.EmitSignal(SignalName.NearestInteractableChanged, ia != null);
    }

    private void BuildPromptUI()
    {
        _promptLayer = new CanvasLayer();
        _promptLayer.Layer = 20;
        AddChild(_promptLayer);

        _promptLabel = new Label();
        _promptLabel.Text = "[E]  Interact";
        _promptLabel.Visible = false;

        // Center-bottom: anchors at horizontal center + bottom edge, offsets define size
        _promptLabel.AnchorLeft   = 0.5f;
        _promptLabel.AnchorRight  = 0.5f;
        _promptLabel.AnchorTop    = 1f;
        _promptLabel.AnchorBottom = 1f;
        _promptLabel.OffsetLeft   = -120f;
        _promptLabel.OffsetRight  =  120f;
        _promptLabel.OffsetTop    = -120f;
        _promptLabel.OffsetBottom =  -80f;
        _promptLabel.HorizontalAlignment = HorizontalAlignment.Center;
        _promptLabel.VerticalAlignment = VerticalAlignment.Center;

        // Style
        _promptLabel.AddThemeColorOverride("font_color", new Color(1f, 0.95f, 0.6f));
        _promptLabel.AddThemeFontSizeOverride("font_size", 18);

        // Rounded panel background
        var styleBox = new StyleBoxFlat();
        styleBox.BgColor = new Color(0.05f, 0.05f, 0.08f, 0.88f);
        styleBox.CornerRadiusTopLeft    = 8;
        styleBox.CornerRadiusTopRight   = 8;
        styleBox.CornerRadiusBottomLeft = 8;
        styleBox.CornerRadiusBottomRight = 8;
        styleBox.ContentMarginLeft  = 14;
        styleBox.ContentMarginRight = 14;
        styleBox.ContentMarginTop   = 6;
        styleBox.ContentMarginBottom = 6;
        _promptLabel.AddThemeStyleboxOverride("normal", styleBox);

        _promptLayer.AddChild(_promptLabel);
    }
}
