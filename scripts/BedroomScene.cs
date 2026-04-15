using Godot;

/// The bedroom — atmospheric, slightly eerie.  Just a door back to the hallway for now.
public partial class BedroomScene : RoomBase
{
    protected override int   RoomWidth  => 1280;
    protected override float FloorY     => 590f;
    protected override Color WallColor  => new Color(0.12f, 0.10f, 0.14f); // cooler, blue-grey

    protected override void SetupRoom()
    {
        SpawnPoints["DefaultSpawn"]     = new Vector2(200f, SpawnY);
        SpawnPoints["SpawnFromHallway"] = new Vector2(200f, SpawnY);

        // ── Bed ───────────────────────────────────────────────────────────────
        AddColorRect(new Rect2(600f, FloorY - 100f, 480f, 20f), new Color(0.5f, 0.35f, 0.20f)); // bed frame top
        AddColorRect(new Rect2(600f, FloorY - 80f, 480f, 80f), new Color(0.38f, 0.28f, 0.18f)); // bed body
        AddColorRect(new Rect2(600f, FloorY - 100f, 60f, 100f), new Color(0.4f, 0.30f, 0.18f)); // headboard left
        AddColorRect(new Rect2(1020f, FloorY - 100f, 60f, 100f), new Color(0.4f, 0.30f, 0.18f)); // headboard right
        // Pillow
        AddColorRect(new Rect2(650f, FloorY - 90f, 140f, 30f), new Color(0.82f, 0.78f, 0.70f));
        AddColorRect(new Rect2(810f, FloorY - 90f, 140f, 30f), new Color(0.80f, 0.76f, 0.68f));
        // Blanket crinkle
        AddColorRect(new Rect2(620f, FloorY - 55f, 440f, 55f), new Color(0.25f, 0.22f, 0.35f)); // dark blanket

        // ── Wardrobe ──────────────────────────────────────────────────────────
        AddColorRect(new Rect2(1080f, FloorY - 260f, 140f, 260f), new Color(0.32f, 0.22f, 0.12f));
        AddColorRect(new Rect2(1080f, FloorY - 262f, 140f, 4f), new Color(0.42f, 0.28f, 0.14f)); // top molding
        AddColorRect(new Rect2(1148f, FloorY - 230f, 2f, 180f), new Color(0.22f, 0.15f, 0.08f)); // center seam
        AddColorRect(new Rect2(1132f, FloorY - 150f, 12f, 12f), new Color(0.7f, 0.55f, 0.1f)); // handle left door
        AddColorRect(new Rect2(1154f, FloorY - 150f, 12f, 12f), new Color(0.7f, 0.55f, 0.1f)); // handle right door

        // ── Side table & candle ───────────────────────────────────────────────
        AddColorRect(new Rect2(560f, FloorY - 80f, 30f, 80f), new Color(0.35f, 0.22f, 0.10f));
        AddColorRect(new Rect2(556f, FloorY - 82f, 38f, 4f), new Color(0.42f, 0.28f, 0.12f));
        // Candle
        AddColorRect(new Rect2(569f, FloorY - 108f, 10f, 28f), new Color(0.92f, 0.88f, 0.78f));
        AddColorRect(new Rect2(570f, FloorY - 112f, 8f, 6f), new Color(0.95f, 0.72f, 0.2f)); // flame

        // ── Door back to hallway ──────────────────────────────────────────────
        AddDoor(
            new Rect2(40f, FloorY - 230f, 100f, 230f),
            "Return to Hallway",
            "res://scenes/hallway.tscn",
            "SpawnFromBedroom"
        );
    }
}
