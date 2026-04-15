using Godot;

/// The living room — a cosy space with fireplace and a sofa.
public partial class LivingRoomScene : RoomBase
{
    protected override int   RoomWidth  => 1280;
    protected override float FloorY     => 590f;
    protected override Color WallColor  => new Color(0.17f, 0.11f, 0.07f); // warm wood tone

    protected override void SetupRoom()
    {
        SpawnPoints["DefaultSpawn"]     = new Vector2(200f, SpawnY);
        SpawnPoints["SpawnFromHallway"] = new Vector2(200f, SpawnY);

        // ── Sofa ──────────────────────────────────────────────────────────────
        AddColorRect(new Rect2(450f, FloorY - 90f, 350f, 90f), new Color(0.35f, 0.22f, 0.35f)); // body
        AddColorRect(new Rect2(440f, FloorY - 120f, 30f, 120f), new Color(0.38f, 0.24f, 0.38f)); // left arm
        AddColorRect(new Rect2(780f, FloorY - 120f, 30f, 120f), new Color(0.38f, 0.24f, 0.38f)); // right arm
        AddColorRect(new Rect2(450f, FloorY - 130f, 350f, 42f), new Color(0.32f, 0.20f, 0.32f)); // back
        // Cushions
        AddColorRect(new Rect2(458f, FloorY - 80f, 100f, 45f), new Color(0.55f, 0.35f, 0.55f));
        AddColorRect(new Rect2(574f, FloorY - 80f, 100f, 45f), new Color(0.52f, 0.33f, 0.52f));
        AddColorRect(new Rect2(690f, FloorY - 80f, 100f, 45f), new Color(0.55f, 0.35f, 0.55f));

        // ── Coffee table ──────────────────────────────────────────────────────
        AddColorRect(new Rect2(500f, FloorY - 45f, 210f, 10f), new Color(0.40f, 0.26f, 0.12f));
        AddColorRect(new Rect2(502f, FloorY - 35f, 8f, 35f), new Color(0.32f, 0.20f, 0.08f));
        AddColorRect(new Rect2(700f, FloorY - 35f, 8f, 35f), new Color(0.32f, 0.20f, 0.08f));

        // ── Fireplace (right wall) ─────────────────────────────────────────────
        AddColorRect(new Rect2(1000f, FloorY - 250f, 200f, 250f), new Color(0.28f, 0.20f, 0.14f)); // surround
        AddColorRect(new Rect2(1020f, FloorY - 200f, 160f, 200f), new Color(0.08f, 0.06f, 0.04f)); // opening
        // Fire flicker (static)
        AddColorRect(new Rect2(1050f, FloorY - 90f, 100f, 90f), new Color(0.8f, 0.3f, 0.05f, 0.7f)); // orange
        AddColorRect(new Rect2(1070f, FloorY - 120f, 60f, 90f), new Color(0.9f, 0.65f, 0.05f, 0.6f)); // yellow
        AddColorRect(new Rect2(1080f, FloorY - 140f, 40f, 60f), new Color(1.0f, 0.9f, 0.5f, 0.5f));  // white core
        // Mantle
        AddColorRect(new Rect2(996f, FloorY - 256f, 208f, 10f), new Color(0.42f, 0.28f, 0.14f));
        // Painting above fireplace
        AddColorRect(new Rect2(1040f, FloorY - 380f, 120f, 90f), new Color(0.22f, 0.18f, 0.30f)); // frame
        AddColorRect(new Rect2(1048f, FloorY - 372f, 104f, 74f), new Color(0.30f, 0.25f, 0.40f)); // canvas

        // ── Bookshelf (left side) ──────────────────────────────────────────────
        AddColorRect(new Rect2(300f, FloorY - 300f, 120f, 300f), new Color(0.30f, 0.18f, 0.08f)); // shelf body
        // Books
        float[] bookWidths  = { 18, 14, 22, 16, 20, 12, 18, 16 };
        Color[] bookColors  = {
            new Color(0.6f, 0.1f, 0.1f), new Color(0.1f, 0.4f, 0.6f),
            new Color(0.2f, 0.5f, 0.2f), new Color(0.5f, 0.4f, 0.1f),
            new Color(0.5f, 0.1f, 0.5f), new Color(0.1f, 0.5f, 0.5f),
            new Color(0.7f, 0.5f, 0.1f), new Color(0.4f, 0.2f, 0.5f)
        };
        float bx = 302f;
        for (int i = 0; i < bookWidths.Length; i++)
        {
            AddColorRect(new Rect2(bx, FloorY - 110f, bookWidths[i] - 2, 90f), bookColors[i]);
            bx += bookWidths[i];
        }
        bx = 302f;
        for (int i = 0; i < bookWidths.Length; i++)
        {
            AddColorRect(new Rect2(bx, FloorY - 210f, bookWidths[i] - 2, 75f), bookColors[(i + 3) % bookColors.Length]);
            bx += bookWidths[i];
        }

        // ── Door back to hallway ──────────────────────────────────────────────
        AddDoor(
            new Rect2(40f, FloorY - 230f, 100f, 230f),
            "Return to Hallway",
            "res://scenes/hallway.tscn",
            "SpawnFromLivingRoom"
        );
    }
}
