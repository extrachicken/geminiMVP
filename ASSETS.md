# Assets

**Art direction:** Anime-inspired 2D side-scrolling adventure game. Dark, moody indoor atmosphere with warm amber candlelight tones. Character sprites on richly painted 2D backgrounds. Color palette: deep warm shadows (#1a0f08), warm amber highlights (#e8a020), aged wallpaper (dark burgundy/gold), wooden floors (dark brown). Clean digital game-engine rendering.

## Provided sprites

| Asset | File | Size | Usage |
|-------|------|------|-------|
| Hero sprite sheet | `sprites/hero_spritesheet.png` | 1274×880 RGBA | Player character — 3 views (front/side/back), each ~424px wide. Side view used for walk, front for idle. |
| Mother sprite | `sprites/mother_sprite.png` | 2480×3508 RGBA | Mother NPC — white background removed. Scaled to ~180px display height. |

## Generated items (procedural)

All item visuals are drawn procedurally in `ItemPickup._Draw()`:
- **Apple** — green circle (#26B226)
- **Key** — amber/gold circle (#D9A619)
- **Note** — pale yellow circle (#E5E099)

## Room visual style

Rooms are painted with `ColorRect` nodes at runtime in each room's `_Ready()` via `RoomBase.AddColorRect()`. No external texture assets needed.
