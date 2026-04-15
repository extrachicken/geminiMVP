using Godot;

/// The kitchen — Mother's domain.  Quest giver and quest receiver live here.
public partial class KitchenScene : RoomBase
{
    protected override int   RoomWidth  => 1280;
    protected override float FloorY     => 590f;
    protected override Color WallColor  => new Color(0.18f, 0.14f, 0.10f);

    protected override void SetupRoom()
    {
        SpawnPoints["DefaultSpawn"]     = new Vector2(640f, SpawnY);
        SpawnPoints["SpawnFromHallway"] = new Vector2(200f, SpawnY);

        // ── Counter / kitchen furniture ───────────────────────────────────────
        float counterX = 900f, counterY = FloorY - 80f;
        AddColorRect(new Rect2(counterX, counterY, 300, 16), new Color(0.45f, 0.30f, 0.15f)); // counter top
        AddColorRect(new Rect2(counterX, counterY + 16, 300, 80), new Color(0.30f, 0.18f, 0.08f)); // cabinet
        // Pot on counter
        AddColorRect(new Rect2(counterX + 20, counterY - 30, 40, 30), new Color(0.35f, 0.35f, 0.40f)); // pot
        AddColorRect(new Rect2(counterX + 10, counterY - 32, 60, 4), new Color(0.28f, 0.28f, 0.32f)); // rim

        // ── Window (high on wall, light spill) ───────────────────────────────
        AddColorRect(new Rect2(1050f, 120f, 140f, 100f), new Color(0.55f, 0.50f, 0.35f, 0.6f)); // glass
        AddColorRect(new Rect2(1046f, 116f, 148f, 4f), new Color(0.45f, 0.28f, 0.12f)); // frame top
        AddColorRect(new Rect2(1046f, 216f, 148f, 4f), new Color(0.45f, 0.28f, 0.12f)); // frame bottom
        AddColorRect(new Rect2(1046f, 116f, 4f, 104f), new Color(0.45f, 0.28f, 0.12f)); // frame left
        AddColorRect(new Rect2(1190f, 116f, 4f, 104f), new Color(0.45f, 0.28f, 0.12f)); // frame right

        // ── Mother NPC ────────────────────────────────────────────────────────
        AddMother(new Vector2(960f, SpawnY));

        // ── Door back to Hallway ──────────────────────────────────────────────
        AddDoor(
            new Rect2(40f, FloorY - 230f, 100f, 230f),
            "Return to Hallway",
            "res://scenes/hallway.tscn",
            "SpawnFromKitchen"
        );

        // ── Ambient lamp ──────────────────────────────────────────────────────
        AddColorRect(new Rect2(600f, 70f, 80f, 30f), new Color(0.85f, 0.70f, 0.3f, 0.4f)); // ceiling lamp glow
        AddColorRect(new Rect2(620f, 70f, 40f, 10f), new Color(0.7f, 0.6f, 0.3f));          // lamp body
    }
}
