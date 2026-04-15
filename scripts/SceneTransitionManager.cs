using Godot;

/// Autoload singleton — handles cross-room transitions with fade-out/fade-in.
public partial class SceneTransitionManager : Node
{
    public static SceneTransitionManager Instance { get; private set; } = null!;

    private CanvasLayer _layer = null!;
    private ColorRect _fade = null!;
    private Label _loadingLabel = null!;
    private bool _transitioning = false;

    public override void _Ready()
    {
        Instance = this;
        BuildFadeUI();
    }

    /// Fade out, load the scene, position player at spawnPoint, fade in.
    public void GoToScene(string scenePath, string spawnPoint = "DefaultSpawn")
    {
        if (_transitioning) return;
        _transitioning = true;

        GameManager.TargetSpawnPoint = spawnPoint;

        // Fade out over 0.4 s
        var tween = CreateTween();
        tween.TweenProperty(_fade, "color", new Color(0, 0, 0, 1), 0.4);
        tween.TweenCallback(Callable.From(() =>
        {
            _loadingLabel.Visible = true;
            GetTree().ChangeSceneToFile(scenePath);

            // Wait a frame before fading back in so the new scene is ready
            var timer = GetTree().CreateTimer(0.1);
            timer.Timeout += () =>
            {
                _loadingLabel.Visible = false;
                var tw2 = CreateTween();
                tw2.TweenProperty(_fade, "color", new Color(0, 0, 0, 0), 0.4);
                tw2.TweenCallback(Callable.From(() => _transitioning = false));
            };
        }));
    }

    private void BuildFadeUI()
    {
        _layer = new CanvasLayer();
        _layer.Layer = 100; // on top of everything
        AddChild(_layer);

        _fade = new ColorRect();
        _fade.Color = new Color(0, 0, 0, 0); // start transparent
        _fade.SetAnchorsPreset(Control.LayoutPreset.FullRect);
        _fade.MouseFilter = Control.MouseFilterEnum.Ignore;
        _layer.AddChild(_fade);

        _loadingLabel = new Label();
        _loadingLabel.Text = "Loading...";
        _loadingLabel.SetAnchorsPreset(Control.LayoutPreset.Center);
        _loadingLabel.Position = new Vector2(-60f, 10f);
        _loadingLabel.Size = new Vector2(120f, 30f);
        _loadingLabel.HorizontalAlignment = HorizontalAlignment.Center;
        _loadingLabel.AddThemeColorOverride("font_color", new Color(0.7f, 0.7f, 0.7f));
        _loadingLabel.AddThemeFontSizeOverride("font_size", 16);
        _loadingLabel.Visible = false;
        _layer.AddChild(_loadingLabel);
    }
}
