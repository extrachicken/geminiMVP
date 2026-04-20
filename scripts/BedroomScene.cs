using Godot;

/// The bedroom — atmospheric, slightly eerie.
public partial class BedroomScene : RoomBase
{
    protected override int   RoomWidth  => 1920;
    protected override float FloorY     => 885f;
    protected override Color WallColor  => new Color(0.12f, 0.10f, 0.14f); // cooler, blue-grey

    protected override void SetupRoom()
    {
        SpawnPoints["DefaultSpawn"]     = new Vector2(300f, SpawnY);
        SpawnPoints["SpawnFromHallway"] = new Vector2(300f, SpawnY);

        // ── Bed ───────────────────────────────────────────────────────────────
        AddColorRect(new Rect2(900f,  FloorY - 150f, 720f, 30f), new Color(0.5f, 0.35f, 0.20f));  // frame top
        AddColorRect(new Rect2(900f,  FloorY - 120f, 720f, 120f), new Color(0.38f, 0.28f, 0.18f)); // bed body
        AddColorRect(new Rect2(900f,  FloorY - 150f, 90f,  150f), new Color(0.4f, 0.30f, 0.18f)); // headboard L
        AddColorRect(new Rect2(1530f, FloorY - 150f, 90f,  150f), new Color(0.4f, 0.30f, 0.18f)); // headboard R
        // Pillows
        AddColorRect(new Rect2(975f,  FloorY - 135f, 210f, 45f), new Color(0.82f, 0.78f, 0.70f));
        AddColorRect(new Rect2(1215f, FloorY - 135f, 210f, 45f), new Color(0.80f, 0.76f, 0.68f));
        // Blanket
        AddColorRect(new Rect2(930f, FloorY - 83f, 660f, 83f), new Color(0.25f, 0.22f, 0.35f));

        // ── Wardrobe ──────────────────────────────────────────────────────────
        AddColorRect(new Rect2(1620f, FloorY - 390f, 210f, 390f), new Color(0.32f, 0.22f, 0.12f));
        AddColorRect(new Rect2(1620f, FloorY - 393f, 210f, 6f),   new Color(0.42f, 0.28f, 0.14f)); // top mold
        AddColorRect(new Rect2(1722f, FloorY - 345f, 3f,   270f), new Color(0.22f, 0.15f, 0.08f)); // center seam
        AddColorRect(new Rect2(1698f, FloorY - 225f, 18f,  18f),  new Color(0.7f, 0.55f, 0.1f));   // handle L
        AddColorRect(new Rect2(1731f, FloorY - 225f, 18f,  18f),  new Color(0.7f, 0.55f, 0.1f));   // handle R

        // ── Side table & candle ───────────────────────────────────────────────
        AddColorRect(new Rect2(840f, FloorY - 120f, 45f,  120f), new Color(0.35f, 0.22f, 0.10f));  // table
        AddColorRect(new Rect2(837f, FloorY - 123f, 57f,  6f),   new Color(0.42f, 0.28f, 0.12f));  // table top
        // Candle
        AddColorRect(new Rect2(853f, FloorY - 162f, 15f, 42f), new Color(0.92f, 0.88f, 0.78f));    // wax
        AddColorRect(new Rect2(854f, FloorY - 168f, 12f,  9f), new Color(0.95f, 0.72f, 0.2f));     // flame

        // ── Door back to hallway ──────────────────────────────────────────────
        AddDoor(
            new Rect2(60f, FloorY - 345f, 150f, 345f),
            "Return to Hallway",
            "res://scenes/hallway.tscn",
            "SpawnFromBedroom"
        );
    }
}
