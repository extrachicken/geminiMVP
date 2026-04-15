using Godot;

/// The hallway — the starting room.
/// Contains: apple item on the table, door to kitchen (left), door to bedroom (right).
public partial class HallwayScene : RoomBase
{
    protected override int   RoomWidth  => 2000;
    protected override float FloorY     => 590f;

    // Richer wallpaper tint for the hallway
    protected override Color WallColor  => new Color(0.20f, 0.12f, 0.08f);

    protected override void SetupRoom()
    {
        // ── Spawn points ──────────────────────────────────────────────────────
        SpawnPoints["DefaultSpawn"]      = new Vector2(640f,  SpawnY);
        SpawnPoints["SpawnFromKitchen"]  = new Vector2(200f,  SpawnY);
        SpawnPoints["SpawnFromBedroom"]  = new Vector2(1800f, SpawnY);

        // ── Orb intro dialogue after a short delay ────────────────────────────
        // Shown only once when entering the hallway for the first time
        var timer = new Timer();
        timer.WaitTime = 1.2f;
        timer.OneShot = true;
        AddChild(timer);
        timer.Timeout += OrbIntro;
        timer.Start();

        // ── Table ─────────────────────────────────────────────────────────────
        // Small wooden table against the back wall, left-center
        float tableX = 750f, tableY = FloorY - 60f;
        AddColorRect(new Rect2(tableX - 40, tableY, 80, 10), new Color(0.4f, 0.25f, 0.1f));   // top
        AddColorRect(new Rect2(tableX - 38, tableY + 10, 6, 50), new Color(0.32f, 0.18f, 0.07f)); // left leg
        AddColorRect(new Rect2(tableX + 32, tableY + 10, 6, 50), new Color(0.32f, 0.18f, 0.07f)); // right leg

        // ── Apple item ────────────────────────────────────────────────────────
        var apple = new ItemData("apple", "Apple", "A fresh green apple.", new Color(0.15f, 0.7f, 0.15f));
        AddItem(apple, new Vector2(tableX, tableY - 14f));

        // ── Key item (on the floor near bedroom door) ─────────────────────────
        var key = new ItemData("key", "Key", "An old brass key.", new Color(0.85f, 0.65f, 0.1f));
        AddItem(key, new Vector2(1500f, FloorY - 14f));

        // ── Note item (on the floor) ──────────────────────────────────────────
        var note = new ItemData("note", "Note", "A crumpled piece of paper.", new Color(0.9f, 0.88f, 0.7f));
        AddItem(note, new Vector2(960f, FloorY - 14f));

        // ── Door to Kitchen (left) ────────────────────────────────────────────
        AddDoor(
            new Rect2(40f, FloorY - 230f, 100f, 230f),
            "Enter Kitchen",
            "res://scenes/kitchen.tscn",
            "SpawnFromHallway"
        );

        // ── Door to Bedroom (right) ───────────────────────────────────────────
        AddDoor(
            new Rect2(1860f, FloorY - 230f, 100f, 230f),
            "Enter Bedroom",
            "res://scenes/bedroom.tscn",
            "SpawnFromHallway"
        );

        // ── Door to Living Room (center-right) ───────────────────────────────
        AddDoor(
            new Rect2(1400f, FloorY - 230f, 100f, 230f),
            "Enter Living Room",
            "res://scenes/living_room.tscn",
            "SpawnFromHallway"
        );

        // ── Ambient wall lamp (painted rectangle as lamp) ─────────────────────
        AddLamp(new Vector2(400f, 200f));
        AddLamp(new Vector2(1200f, 200f));
        AddLamp(new Vector2(1800f, 200f));
    }

    private void OrbIntro()
    {
        // Only show if quest hasn't started yet
        if (QuestManager.Instance.IsQuestActive("HelpMother") ||
            QuestManager.Instance.IsQuestComplete("HelpMother"))
            return;

        var lines = new System.Collections.Generic.List<DialogueLine>
        {
            new("Orb",    "This house feels strange... maybe we should check the kitchen first."),
            new("Player", "You always say that."),
            new("Orb",    "And I'm usually right."),
            new("Orb",    "Also — is that an apple on the table? Might be useful."),
        };
        DialogueManager.Instance.StartDialogue(lines);
    }

    private void AddLamp(Vector2 pos)
    {
        // Simple wall sconce drawing (rectangle + glow tint)
        AddColorRect(new Rect2(pos.X - 6, pos.Y, 12, 20), new Color(0.6f, 0.45f, 0.2f), -6); // bracket
        AddColorRect(new Rect2(pos.X - 10, pos.Y + 20, 20, 16), new Color(0.9f, 0.75f, 0.3f), -5); // lamp
        // Glow cone (translucent triangle approximated as wide rect)
        AddColorRect(new Rect2(pos.X - 40, pos.Y + 36, 80, 80), new Color(0.9f, 0.75f, 0.3f, 0.06f), -7);
    }
}
