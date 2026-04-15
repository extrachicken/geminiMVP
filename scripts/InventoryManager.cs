using Godot;
using System.Collections.Generic;

/// Item data — a plain data class (not a Godot object).
public class ItemData
{
    public string ItemId      { get; set; }
    public string Name        { get; set; }
    public string Description { get; set; }
    public Color  IconColor   { get; set; }

    public ItemData(string id, string name, string description, Color iconColor)
    {
        ItemId      = id;
        Name        = name;
        Description = description;
        IconColor   = iconColor;
    }
}

/// Autoload singleton — manages the 6-slot inventory and renders the hotbar HUD.
public partial class InventoryManager : Node
{
    public static InventoryManager Instance { get; private set; } = null!;

    public const int SlotCount = 6;

    private ItemData?[] _slots = new ItemData?[SlotCount];
    private int _selectedSlot = 0;

    // HUD nodes
    private CanvasLayer _hudLayer = null!;
    private Panel[] _slotPanels = new Panel[SlotCount];
    private ColorRect[] _slotIcons = new ColorRect[SlotCount];
    private Label[] _slotLabels = new Label[SlotCount];

    // Slot visual constants
    private const float SlotSize = 64f;
    private const float SlotGap  = 8f;
    private const float HotbarY  = 640f; // from top of viewport
    private static readonly Color EmptySlotColor      = new Color(0.18f, 0.18f, 0.22f, 0.92f);
    private static readonly Color SelectedBorderColor  = new Color(0.95f, 0.82f, 0.15f);
    private static readonly Color DefaultBorderColor   = new Color(0.35f, 0.35f, 0.42f);

    public override void _Ready()
    {
        Instance = this;
        BuildHotbarUI();
        _hudLayer.Visible = false; // hidden until a room scene shows it
    }

    // ─── Public API ───────────────────────────────────────────────────────────

    public bool AddItem(ItemData item)
    {
        for (int i = 0; i < SlotCount; i++)
        {
            if (_slots[i] == null)
            {
                _slots[i] = item;
                RefreshSlotVisual(i);
                return true;
            }
        }
        return false; // inventory full
    }

    public bool HasItem(string itemId)
    {
        foreach (var slot in _slots)
            if (slot != null && slot.ItemId == itemId) return true;
        return false;
    }

    public bool RemoveItem(string itemId)
    {
        for (int i = 0; i < SlotCount; i++)
        {
            if (_slots[i] != null && _slots[i]!.ItemId == itemId)
            {
                _slots[i] = null;
                RefreshSlotVisual(i);
                return true;
            }
        }
        return false;
    }

    public void SelectSlot(int index)
    {
        int prev = _selectedSlot;
        _selectedSlot = Mathf.Clamp(index, 0, SlotCount - 1);
        RefreshSlotBorder(prev);
        RefreshSlotBorder(_selectedSlot);
    }

    public void SelectNext() => SelectSlot((_selectedSlot + 1) % SlotCount);
    public void SelectPrev() => SelectSlot((_selectedSlot - 1 + SlotCount) % SlotCount);

    public void SetHUDVisible(bool visible) => _hudLayer.Visible = visible;

    public ItemData? GetSelected() => _slots[_selectedSlot];

    // ─── UI Building ─────────────────────────────────────────────────────────

    private void BuildHotbarUI()
    {
        _hudLayer = new CanvasLayer();
        _hudLayer.Layer = 10;
        AddChild(_hudLayer);

        float totalWidth = SlotCount * SlotSize + (SlotCount - 1) * SlotGap;
        float startX = (1280f - totalWidth) / 2f;

        // Background panel
        var bg = new Panel();
        var bgStyle = new StyleBoxFlat();
        bgStyle.BgColor = new Color(0.06f, 0.06f, 0.10f, 0.90f);
        bgStyle.CornerRadiusTopLeft = bgStyle.CornerRadiusTopRight = 10;
        bgStyle.CornerRadiusBottomLeft = bgStyle.CornerRadiusBottomRight = 10;
        bgStyle.ContentMarginLeft = bgStyle.ContentMarginRight = 10;
        bgStyle.ContentMarginTop = bgStyle.ContentMarginBottom = 10;
        bg.AddThemeStyleboxOverride("panel", bgStyle);
        bg.Position = new Vector2(startX - 10f, HotbarY - 10f);
        bg.Size = new Vector2(totalWidth + 20f, SlotSize + 20f);
        _hudLayer.AddChild(bg);

        for (int i = 0; i < SlotCount; i++)
        {
            float x = startX + i * (SlotSize + SlotGap);

            var panel = new Panel();
            var style = BuildSlotStyle(i == _selectedSlot);
            panel.AddThemeStyleboxOverride("panel", style);
            panel.Position = new Vector2(x, HotbarY);
            panel.Size = new Vector2(SlotSize, SlotSize);
            _hudLayer.AddChild(panel);
            _slotPanels[i] = panel;

            // Icon color rect (hidden when empty)
            var icon = new ColorRect();
            icon.Color = EmptySlotColor;
            icon.Position = new Vector2(x + 8f, HotbarY + 8f);
            icon.Size = new Vector2(SlotSize - 16f, SlotSize - 16f);
            icon.Visible = false;
            _hudLayer.AddChild(icon);
            _slotIcons[i] = icon;

            // Item name label (tiny, below icon)
            var lbl = new Label();
            lbl.Position = new Vector2(x, HotbarY + SlotSize - 18f);
            lbl.Size = new Vector2(SlotSize, 18f);
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.AddThemeColorOverride("font_color", Colors.WhiteSmoke);
            lbl.AddThemeFontSizeOverride("font_size", 9);
            lbl.ClipText = true;
            lbl.Text = "";
            _hudLayer.AddChild(lbl);
            _slotLabels[i] = lbl;
        }

        // Slot number hints (1-6)
        for (int i = 0; i < SlotCount; i++)
        {
            float x = startX + i * (SlotSize + SlotGap);
            var num = new Label();
            num.Text = (i + 1).ToString();
            num.Position = new Vector2(x + 2f, HotbarY + 2f);
            num.Size = new Vector2(20f, 16f);
            num.AddThemeColorOverride("font_color", new Color(1, 1, 1, 0.4f));
            num.AddThemeFontSizeOverride("font_size", 10);
            _hudLayer.AddChild(num);
        }
    }

    private StyleBoxFlat BuildSlotStyle(bool selected)
    {
        var s = new StyleBoxFlat();
        s.BgColor = new Color(0.12f, 0.12f, 0.18f, 0.95f);
        s.BorderColor = selected ? SelectedBorderColor : DefaultBorderColor;
        s.BorderWidthBottom = s.BorderWidthTop = s.BorderWidthLeft = s.BorderWidthRight = selected ? 2 : 1;
        s.CornerRadiusTopLeft = s.CornerRadiusTopRight = 6;
        s.CornerRadiusBottomLeft = s.CornerRadiusBottomRight = 6;
        return s;
    }

    private void RefreshSlotBorder(int i)
    {
        if (i < 0 || i >= SlotCount) return;
        _slotPanels[i].AddThemeStyleboxOverride("panel", BuildSlotStyle(i == _selectedSlot));
    }

    private void RefreshSlotVisual(int i)
    {
        var item = _slots[i];
        if (item != null)
        {
            _slotIcons[i].Color = item.IconColor;
            _slotIcons[i].Visible = true;
            _slotLabels[i].Text = item.Name;
        }
        else
        {
            _slotIcons[i].Visible = false;
            _slotLabels[i].Text = "";
        }
    }
}
