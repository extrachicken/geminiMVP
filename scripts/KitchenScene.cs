using Godot;

/// The kitchen — Mother's domain. Quest giver and quest receiver live here.
public partial class KitchenScene : RoomBase
{
    protected override int   RoomWidth  => 1920;
    protected override float FloorY     => 885f;
    protected override Color WallColor  => new Color(0.18f, 0.14f, 0.10f);

    protected override void SetupRoom()
    {
        SpawnPoints["DefaultSpawn"]     = new Vector2(960f, SpawnY);
        SpawnPoints["SpawnFromHallway"] = new Vector2(300f, SpawnY);

        // ── Counter / kitchen furniture ───────────────────────────────────────
        float counterX = 1350f, counterY = FloorY - 120f;
        AddColorRect(new Rect2(counterX, counterY, 450, 24), new Color(0.45f, 0.30f, 0.15f));          // counter top
        AddColorRect(new Rect2(counterX, counterY + 24, 450, 120), new Color(0.30f, 0.18f, 0.08f));    // cabinet
        // Pot on counter
        AddColorRect(new Rect2(counterX + 30, counterY - 45, 60, 45), new Color(0.35f, 0.35f, 0.40f)); // pot
        AddColorRect(new Rect2(counterX + 15, counterY - 48, 90, 6), new Color(0.28f, 0.28f, 0.32f));  // rim

        // ── Window (high on wall) ─────────────────────────────────────────────
        AddColorRect(new Rect2(1575f, 180f, 210f, 150f), new Color(0.55f, 0.50f, 0.35f, 0.6f));  // glass
        AddColorRect(new Rect2(1569f, 174f, 222f, 6f),   new Color(0.45f, 0.28f, 0.12f));        // frame top
        AddColorRect(new Rect2(1569f, 324f, 222f, 6f),   new Color(0.45f, 0.28f, 0.12f));        // frame bottom
        AddColorRect(new Rect2(1569f, 174f, 6f,  156f),  new Color(0.45f, 0.28f, 0.12f));        // frame left
        AddColorRect(new Rect2(1785f, 174f, 6f,  156f),  new Color(0.45f, 0.28f, 0.12f));        // frame right

        // ── Mother NPC ────────────────────────────────────────────────────────
        AddMother(new Vector2(1440f, SpawnY));

        // ── Door back to Hallway ──────────────────────────────────────────────
        AddDoor(
            new Rect2(60f, FloorY - 345f, 150f, 345f),
            "Return to Hallway",
            "res://scenes/hallway.tscn",
            "SpawnFromKitchen"
        );

        // ── Ambient ceiling lamp ──────────────────────────────────────────────
        AddColorRect(new Rect2(900f, 105f, 120f, 45f), new Color(0.85f, 0.70f, 0.3f, 0.4f)); // glow
        AddColorRect(new Rect2(930f, 105f,  60f, 15f), new Color(0.7f,  0.6f,  0.3f));       // lamp body
    }
}
