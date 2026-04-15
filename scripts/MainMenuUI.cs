using Godot;

/// Main menu scene controller.
/// Handles title display, Game/Settings/Quit buttons, and the settings panel.
public partial class MainMenuUI : Control
{
    // Settings panel controls
    private Control     _settingsPanel = null!;
    private HSlider     _masterSlider  = null!;
    private HSlider     _musicSlider   = null!;
    private HSlider     _sfxSlider     = null!;
    private CheckButton _fullscreenBtn = null!;
    private HSlider     _sensitivitySlider = null!;

    private bool _settingsOpen = false;

    // Colors
    private static readonly Color PanelBg     = new Color(0.05f, 0.04f, 0.07f, 0.97f);
    private static readonly Color ButtonBg    = new Color(0.14f, 0.10f, 0.18f, 1f);
    private static readonly Color ButtonHover = new Color(0.24f, 0.18f, 0.30f, 1f);
    private static readonly Color Gold        = new Color(0.95f, 0.82f, 0.2f);

    public override void _Ready()
    {
        // Full-screen background
        AnchorLeft = 0; AnchorRight = 1; AnchorTop = 0; AnchorBottom = 1;

        // Background (uses stretch mode to fill screen)
        var bg = new ColorRect();
        bg.Color = new Color(0.04f, 0.03f, 0.06f);
        bg.AnchorLeft = 0; bg.AnchorRight = 1; bg.AnchorTop = 0; bg.AnchorBottom = 1;
        AddChild(bg);

        // Decorative vertical stripe
        var stripe = new ColorRect();
        stripe.Color = new Color(0.5f, 0.4f, 0.1f, 0.06f);
        stripe.Position = new Vector2(500, 0);
        stripe.Size = new Vector2(280, 720);
        AddChild(stripe);

        // Title — no anchors, absolute position within full-screen parent
        var title = new Label();
        title.Text = "THE  HOUSE";
        title.Position = new Vector2(0, 160);
        title.Size = new Vector2(1280, 80);
        title.HorizontalAlignment = HorizontalAlignment.Center;
        title.AddThemeColorOverride("font_color", Gold);
        title.AddThemeFontSizeOverride("font_size", 54);
        AddChild(title);

        // Subtitle
        var subtitle = new Label();
        subtitle.Text = "A story in rooms";
        subtitle.Position = new Vector2(0, 230);
        subtitle.Size = new Vector2(1280, 30);
        subtitle.HorizontalAlignment = HorizontalAlignment.Center;
        subtitle.AddThemeColorOverride("font_color", new Color(0.7f, 0.65f, 0.55f));
        subtitle.AddThemeFontSizeOverride("font_size", 18);
        AddChild(subtitle);

        // Divider
        var div = new ColorRect();
        div.Color = new Color(0.6f, 0.5f, 0.15f, 0.5f);
        div.Position = new Vector2(530, 272);
        div.Size = new Vector2(220, 1);
        AddChild(div);

        // Buttons
        float btnCenterX = 640f;
        float btnY = 310f;
        float btnSpacing = 68f;

        MakeButton("  Play",     new Vector2(btnCenterX, btnY),                OnPlay);
        MakeButton("  Settings", new Vector2(btnCenterX, btnY + btnSpacing),   OnSettings);
        MakeButton("  Quit",     new Vector2(btnCenterX, btnY + btnSpacing*2), OnQuit);

        // Version label
        var ver = new Label();
        ver.Text = "v0.1 MVP";
        ver.SetAnchorsPreset(LayoutPreset.BottomRight);
        ver.Position = new Vector2(-100, -30);
        ver.Size = new Vector2(90, 24);
        ver.AddThemeColorOverride("font_color", new Color(0.4f, 0.4f, 0.4f));
        ver.AddThemeFontSizeOverride("font_size", 12);
        AddChild(ver);

        // Settings panel (hidden by default)
        _settingsPanel = BuildSettingsPanel();
        _settingsPanel.Visible = false;
        AddChild(_settingsPanel);
    }

    // ─── Button factory ───────────────────────────────────────────────────────

    private void MakeButton(string text, Vector2 center, System.Action callback)
    {
        const float W = 220f, H = 50f;
        var btn = new Button();
        btn.Text = text;
        btn.Position = new Vector2(center.X - W / 2f, center.Y);
        btn.Size = new Vector2(W, H);

        var normalStyle = ButtonStyleBox(ButtonBg);
        var hoverStyle  = ButtonStyleBox(ButtonHover);
        btn.AddThemeStyleboxOverride("normal", normalStyle);
        btn.AddThemeStyleboxOverride("hover",  hoverStyle);
        btn.AddThemeStyleboxOverride("pressed", ButtonStyleBox(new Color(0.08f, 0.06f, 0.12f)));
        btn.AddThemeColorOverride("font_color", new Color(0.92f, 0.90f, 0.86f));
        btn.AddThemeFontSizeOverride("font_size", 20);

        btn.Pressed += () => callback();
        AddChild(btn);
    }

    private StyleBoxFlat ButtonStyleBox(Color bg)
    {
        var s = new StyleBoxFlat();
        s.BgColor = bg;
        s.CornerRadiusTopLeft = s.CornerRadiusTopRight = 6;
        s.CornerRadiusBottomLeft = s.CornerRadiusBottomRight = 6;
        s.BorderColor = new Color(0.6f, 0.5f, 0.15f, 0.7f);
        s.BorderWidthBottom = s.BorderWidthTop = s.BorderWidthLeft = s.BorderWidthRight = 1;
        s.ContentMarginLeft = 20;
        return s;
    }

    // ─── Button callbacks ─────────────────────────────────────────────────────

    private void OnPlay()
    {
        SceneTransitionManager.Instance.GoToScene("res://scenes/hallway.tscn", "DefaultSpawn");
    }

    private void OnSettings()
    {
        _settingsOpen = !_settingsOpen;
        _settingsPanel.Visible = _settingsOpen;
        if (_settingsOpen) RefreshSettingsControls();
    }

    private void OnQuit()
    {
        GetTree().Quit();
    }

    // ─── Settings panel ───────────────────────────────────────────────────────

    private Control BuildSettingsPanel()
    {
        var panel = new Panel();
        panel.Position = new Vector2(380f, 100f);
        panel.Size = new Vector2(520f, 480f);

        var style = new StyleBoxFlat();
        style.BgColor = PanelBg;
        style.CornerRadiusTopLeft = style.CornerRadiusTopRight = 12;
        style.CornerRadiusBottomLeft = style.CornerRadiusBottomRight = 12;
        style.BorderColor = new Color(0.6f, 0.5f, 0.15f, 0.8f);
        style.BorderWidthBottom = style.BorderWidthTop = style.BorderWidthLeft = style.BorderWidthRight = 1;
        panel.AddThemeStyleboxOverride("panel", style);

        var vbox = new VBoxContainer();
        vbox.Position = new Vector2(28, 24);
        vbox.Size = new Vector2(464, 432);
        vbox.AddThemeConstantOverride("separation", 18);
        panel.AddChild(vbox);

        // Title
        var title = new Label();
        title.Text = "Settings";
        title.AddThemeColorOverride("font_color", Gold);
        title.AddThemeFontSizeOverride("font_size", 22);
        vbox.AddChild(title);

        _masterSlider     = AddSlider(vbox, "Master Volume",     0, 1, 0.01f);
        _musicSlider      = AddSlider(vbox, "Music Volume",      0, 1, 0.01f);
        _sfxSlider        = AddSlider(vbox, "SFX Volume",        0, 1, 0.01f);
        _sensitivitySlider = AddSlider(vbox, "Mouse Sensitivity", 0.1f, 5f, 0.1f);

        // Fullscreen toggle
        var fsRow = new HBoxContainer();
        var fsLabel = new Label();
        fsLabel.Text = "Fullscreen";
        fsLabel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        fsLabel.AddThemeColorOverride("font_color", Colors.WhiteSmoke);
        fsRow.AddChild(fsLabel);
        _fullscreenBtn = new CheckButton();
        _fullscreenBtn.Toggled += (pressed) =>
        {
            SettingsManager.Instance.Fullscreen = pressed;
            SettingsManager.Instance.SaveSettings();
        };
        fsRow.AddChild(_fullscreenBtn);
        vbox.AddChild(fsRow);

        // Close button
        var closeBtn = new Button();
        closeBtn.Text = "Close";
        closeBtn.AddThemeStyleboxOverride("normal", ButtonStyleBox(ButtonBg));
        closeBtn.AddThemeStyleboxOverride("hover",  ButtonStyleBox(ButtonHover));
        closeBtn.AddThemeColorOverride("font_color", Colors.WhiteSmoke);
        closeBtn.Pressed += () => { _settingsPanel.Visible = false; _settingsOpen = false; };
        vbox.AddChild(closeBtn);

        return panel;
    }

    private HSlider AddSlider(VBoxContainer parent, string labelText, float min, float max, float step)
    {
        var row = new VBoxContainer();
        row.AddThemeConstantOverride("separation", 4);

        var lbl = new Label();
        lbl.Text = labelText;
        lbl.AddThemeColorOverride("font_color", new Color(0.85f, 0.85f, 0.85f));
        lbl.AddThemeFontSizeOverride("font_size", 13);
        row.AddChild(lbl);

        var slider = new HSlider();
        slider.MinValue = min;
        slider.MaxValue = max;
        slider.Step = step;
        slider.CustomMinimumSize = new Vector2(0, 24);
        slider.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

        slider.ValueChanged += (val) =>
        {
            if (labelText == "Master Volume")       { SettingsManager.Instance.MasterVolume = (float)val; SettingsManager.Instance.SaveSettings(); }
            else if (labelText == "Music Volume")   { SettingsManager.Instance.MusicVolume  = (float)val; SettingsManager.Instance.SaveSettings(); }
            else if (labelText == "SFX Volume")     { SettingsManager.Instance.SfxVolume    = (float)val; SettingsManager.Instance.SaveSettings(); }
            else if (labelText == "Mouse Sensitivity") { SettingsManager.Instance.MouseSensitivity = (float)val; SettingsManager.Instance.SaveSettings(); }
        };

        row.AddChild(slider);
        parent.AddChild(row);
        return slider;
    }

    private void RefreshSettingsControls()
    {
        var sm = SettingsManager.Instance;
        _masterSlider.Value      = sm.MasterVolume;
        _musicSlider.Value       = sm.MusicVolume;
        _sfxSlider.Value         = sm.SfxVolume;
        _sensitivitySlider.Value = sm.MouseSensitivity;
        _fullscreenBtn.ButtonPressed = sm.Fullscreen;
    }
}
