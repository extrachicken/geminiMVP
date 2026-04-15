using Godot;
using System.Collections.Generic;

/// Quest state data.
public class QuestData
{
    public string QuestId        { get; set; }
    public string Title          { get; set; }
    public string Objective      { get; set; }
    public string RequiredItemId { get; set; }
    public bool   IsActive       { get; private set; }
    public bool   IsComplete     { get; private set; }

    public QuestData(string id, string title, string objective, string requiredItemId = "")
    {
        QuestId        = id;
        Title          = title;
        Objective      = objective;
        RequiredItemId = requiredItemId;
    }

    public void Start()    { IsActive = true;  IsComplete = false; }
    public void Complete() { IsActive = false; IsComplete = true;  }
}

/// Autoload singleton — manages quest state and renders the quest tracker UI.
public partial class QuestManager : Node
{
    public static QuestManager Instance { get; private set; } = null!;

    private Dictionary<string, QuestData> _quests = new();

    // Quest tracker UI
    private CanvasLayer _uiLayer = null!;
    private Panel _panel = null!;
    private Label _titleLabel = null!;
    private Label _objectiveLabel = null!;
    private Label _statusLabel = null!;

    public override void _Ready()
    {
        Instance = this;
        BuildQuestUI();
    }

    // ─── Public API ───────────────────────────────────────────────────────────

    public void StartQuest(QuestData quest)
    {
        _quests[quest.QuestId] = quest;
        quest.Start();
        RefreshUI();
    }

    public bool CompleteQuest(string questId)
    {
        if (_quests.TryGetValue(questId, out var q) && q.IsActive)
        {
            q.Complete();
            RefreshUI();
            return true;
        }
        return false;
    }

    public bool IsQuestActive(string questId) =>
        _quests.TryGetValue(questId, out var q) && q.IsActive;

    public bool IsQuestComplete(string questId) =>
        _quests.TryGetValue(questId, out var q) && q.IsComplete;

    public QuestData? GetQuest(string questId) =>
        _quests.TryGetValue(questId, out var q) ? q : null;

    public void SetHUDVisible(bool visible) => _uiLayer.Visible = visible;

    // ─── UI ──────────────────────────────────────────────────────────────────

    private void BuildQuestUI()
    {
        _uiLayer = new CanvasLayer();
        _uiLayer.Layer = 10;
        AddChild(_uiLayer);

        _panel = new Panel();
        var style = new StyleBoxFlat();
        style.BgColor = new Color(0.05f, 0.05f, 0.09f, 0.88f);
        style.CornerRadiusTopLeft = style.CornerRadiusTopRight = 8;
        style.CornerRadiusBottomLeft = style.CornerRadiusBottomRight = 8;
        style.BorderColor = new Color(0.6f, 0.5f, 0.15f, 0.8f);
        style.BorderWidthBottom = style.BorderWidthTop = style.BorderWidthLeft = style.BorderWidthRight = 1;
        style.ContentMarginLeft = style.ContentMarginRight = 12;
        style.ContentMarginTop = style.ContentMarginBottom = 10;
        _panel.AddThemeStyleboxOverride("panel", style);

        // Top-right corner: anchor right, fixed pixel offsets
        _panel.AnchorLeft   = 1f;
        _panel.AnchorRight  = 1f;
        _panel.AnchorTop    = 0f;
        _panel.AnchorBottom = 0f;
        _panel.OffsetLeft   = -270f;
        _panel.OffsetRight  = -20f;
        _panel.OffsetTop    = 20f;
        _panel.OffsetBottom = 110f;
        _panel.Visible = false;
        _uiLayer.AddChild(_panel);

        var vbox = new VBoxContainer();
        vbox.Position = new Vector2(12, 10);
        vbox.Size = new Vector2(226f, 70f);
        _panel.AddChild(vbox);

        var headerRow = new HBoxContainer();
        vbox.AddChild(headerRow);

        var questIcon = new Label();
        questIcon.Text = "⬡";
        questIcon.AddThemeColorOverride("font_color", new Color(0.95f, 0.82f, 0.2f));
        questIcon.AddThemeFontSizeOverride("font_size", 14);
        headerRow.AddChild(questIcon);

        _titleLabel = new Label();
        _titleLabel.Text = "";
        _titleLabel.AddThemeColorOverride("font_color", new Color(0.95f, 0.88f, 0.5f));
        _titleLabel.AddThemeFontSizeOverride("font_size", 13);
        _titleLabel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        headerRow.AddChild(_titleLabel);

        _objectiveLabel = new Label();
        _objectiveLabel.Text = "";
        _objectiveLabel.AddThemeColorOverride("font_color", new Color(0.85f, 0.85f, 0.85f));
        _objectiveLabel.AddThemeFontSizeOverride("font_size", 11);
        _objectiveLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
        vbox.AddChild(_objectiveLabel);

        _statusLabel = new Label();
        _statusLabel.Text = "";
        _statusLabel.AddThemeColorOverride("font_color", new Color(0.4f, 0.9f, 0.4f));
        _statusLabel.AddThemeFontSizeOverride("font_size", 11);
        vbox.AddChild(_statusLabel);
    }

    private void RefreshUI()
    {
        // Show the most recent active quest, then any complete quest
        QuestData? show = null;
        foreach (var q in _quests.Values)
            if (q.IsActive) { show = q; break; }
        if (show == null)
            foreach (var q in _quests.Values)
                if (q.IsComplete) { show = q; break; }

        if (show == null)
        {
            _panel.Visible = false;
            return;
        }

        _panel.Visible = true;
        _titleLabel.Text = " " + show.Title;
        _objectiveLabel.Text = show.Objective;

        if (show.IsComplete)
        {
            _statusLabel.Text = "✓ Complete!";
            _statusLabel.AddThemeColorOverride("font_color", new Color(0.4f, 0.9f, 0.4f));
        }
        else
        {
            _statusLabel.Text = "";
        }
    }
}
