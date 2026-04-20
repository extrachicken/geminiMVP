using Godot;

/// The living room — a cosy space with fireplace and a sofa.
public partial class LivingRoomScene : RoomBase
{
    protected override int   RoomWidth  => 1920;
    protected override float FloorY     => 885f;
    protected override Color WallColor  => new Color(0.17f, 0.11f, 0.07f); // warm wood tone

    protected override void SetupRoom()
    {
        SpawnPoints["DefaultSpawn"]     = new Vector2(300f, SpawnY);
        SpawnPoints["SpawnFromHallway"] = new Vector2(300f, SpawnY);

        // ── Sofa ──────────────────────────────────────────────────────────────
        AddColorRect(new Rect2(675f,  FloorY - 135f, 525f, 135f), new Color(0.35f, 0.22f, 0.35f)); // body
        AddColorRect(new Rect2(660f,  FloorY - 180f, 45f,  180f), new Color(0.38f, 0.24f, 0.38f)); // left arm
        AddColorRect(new Rect2(1170f, FloorY - 180f, 45f,  180f), new Color(0.38f, 0.24f, 0.38f)); // right arm
        AddColorRect(new Rect2(675f,  FloorY - 195f, 525f, 63f),  new Color(0.32f, 0.20f, 0.32f)); // back
        // Cushions
        AddColorRect(new Rect2(687f,  FloorY - 120f, 150f, 68f), new Color(0.55f, 0.35f, 0.55f));
        AddColorRect(new Rect2(861f,  FloorY - 120f, 150f, 68f), new Color(0.52f, 0.33f, 0.52f));
        AddColorRect(new Rect2(1035f, FloorY - 120f, 150f, 68f), new Color(0.55f, 0.35f, 0.55f));

        // ── Coffee table ──────────────────────────────────────────────────────
        AddColorRect(new Rect2(750f,  FloorY - 68f, 315f, 15f), new Color(0.40f, 0.26f, 0.12f)); // top
        AddColorRect(new Rect2(753f,  FloorY - 53f, 12f,  53f), new Color(0.32f, 0.20f, 0.08f)); // leg L
        AddColorRect(new Rect2(1050f, FloorY - 53f, 12f,  53f), new Color(0.32f, 0.20f, 0.08f)); // leg R

        // ── Fireplace (right wall) ─────────────────────────────────────────────
        AddColorRect(new Rect2(1500f, FloorY - 375f, 300f, 375f), new Color(0.28f, 0.20f, 0.14f)); // surround
        AddColorRect(new Rect2(1530f, FloorY - 300f, 240f, 300f), new Color(0.08f, 0.06f, 0.04f)); // opening
        // Fire
        AddColorRect(new Rect2(1575f, FloorY - 135f, 150f, 135f), new Color(0.8f, 0.3f,  0.05f, 0.7f)); // orange
        AddColorRect(new Rect2(1605f, FloorY - 180f, 90f,  135f), new Color(0.9f, 0.65f, 0.05f, 0.6f)); // yellow
        AddColorRect(new Rect2(1620f, FloorY - 210f, 60f,  90f),  new Color(1.0f, 0.9f,  0.5f,  0.5f)); // white core
        // Mantle
        AddColorRect(new Rect2(1494f, FloorY - 384f, 312f, 15f),  new Color(0.42f, 0.28f, 0.14f));
        // Painting above fireplace
        AddColorRect(new Rect2(1560f, FloorY - 570f, 180f, 135f), new Color(0.22f, 0.18f, 0.30f)); // frame
        AddColorRect(new Rect2(1572f, FloorY - 558f, 156f, 111f), new Color(0.30f, 0.25f, 0.40f)); // canvas

        // ── Bookshelf (left side) ──────────────────────────────────────────────
        AddColorRect(new Rect2(450f, FloorY - 450f, 180f, 450f), new Color(0.30f, 0.18f, 0.08f)); // shelf body
        // Books — shelf row 1 (lower)
        float[] bookWidths = { 27f, 21f, 33f, 24f, 30f, 18f, 27f, 24f };
        Color[] bookColors = {
            new Color(0.6f, 0.1f, 0.1f), new Color(0.1f, 0.4f, 0.6f),
            new Color(0.2f, 0.5f, 0.2f), new Color(0.5f, 0.4f, 0.1f),
            new Color(0.5f, 0.1f, 0.5f), new Color(0.1f, 0.5f, 0.5f),
            new Color(0.7f, 0.5f, 0.1f), new Color(0.4f, 0.2f, 0.5f)
        };
        float bx = 453f;
        for (int i = 0; i < bookWidths.Length; i++)
        {
            AddColorRect(new Rect2(bx, FloorY - 165f, bookWidths[i] - 3f, 135f), bookColors[i]);
            bx += bookWidths[i];
        }
        // Books — shelf row 2 (upper)
        bx = 453f;
        for (int i = 0; i < bookWidths.Length; i++)
        {
            AddColorRect(new Rect2(bx, FloorY - 315f, bookWidths[i] - 3f, 113f), bookColors[(i + 3) % bookColors.Length]);
            bx += bookWidths[i];
        }

        // ── Door back to hallway ──────────────────────────────────────────────
        AddDoor(
            new Rect2(60f, FloorY - 345f, 150f, 345f),
            "Return to Hallway",
            "res://scenes/hallway.tscn",
            "SpawnFromLivingRoom"
        );
    }
}
