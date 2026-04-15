# Game Plan: The House

## Game Description

Third-person 2D side-scrolling indoor exploration game. Player starts in a hallway, explores a house with multiple rooms connected by doors, interacts with an AI companion orb and a mother NPC, completes a simple fetch quest, and manages a small inventory.

## Risk Tasks

### 1. Sprite animation from design-sheet source
- **Why isolated:** Hero sprite sheet is a CHARACTER DESIGN SHEET (3 views: front/side/back) rather than a multi-frame animation sheet. Region extraction must be pixel-accurate; wrong frame = wrong character view.
- **Approach:** Divide sheet width by 3 → frameWidth per view. Use Sprite2D.RegionRect to select front (idle) vs side (walk). Flip horizontally for left movement.
- **Verify:** Player shows front view when idle, side view when walking right, mirrored side view when walking left. No magenta/blank region visible.

### 2. Camera2D lerp from-origin swoop
- **Why isolated:** Camera lerp initialised before first `_Process()` tick swoops from (0,0). `_initialized` flag must snap on frame 1.
- **Approach:** In `PlayerController._Ready()`, snap `Camera2D.GlobalPosition = GlobalPosition` before physics runs.
- **Verify:** No visible camera swoop when entering any room.

## Main Build

- [x] Project scaffold: project.godot, TheHouse.csproj, .gitignore
- [x] Autoloads: SettingsManager, GameManager, InventoryManager, QuestManager, DialogueManager, SceneTransitionManager
- [x] Player scene: CharacterBody2D + sprite + camera + interact area
- [x] Orb companion scene: Node2D with OrbFollower
- [x] RoomBase: shared room geometry, player/orb spawning, door/item helpers
- [x] Room scenes: HallwayScene, KitchenScene, BedroomScene, LivingRoomScene
- [x] InteractableBase + DoorInteraction + ItemPickup + MotherNPCController
- [x] Main menu scene: title, Play/Settings/Quit buttons
- [x] Settings panel: volume sliders, fullscreen toggle, sensitivity
- [x] Inventory HUD: 6-slot hotbar, mouse wheel / 1-6 keys
- [x] Quest tracker UI: top-right panel, shows active/complete quest
- [x] Dialogue box UI: bottom panel, speaker + text, E/click to advance
- [x] Interact prompt: GameManager CanvasLayer label shows "[E] Action" when near interactable
- [x] Scene builders: BuildPlayer, BuildOrb, BuildHallway, BuildKitchen, BuildBedroom, BuildLivingRoom, BuildMainMenu
- [x] dotnet build + godot --headless --import
- [x] Run scene builders in order
- [x] godot --headless --quit verification
- [x] HUD visibility: hotbar/prompt hidden on main menu, shown in game rooms
- [x] Hero sprite white background removed (PIL threshold >230)

## Verify

- Player walks left/right with correct sprite flip
- Inventory slots fill when items are picked up; mouse wheel cycles selection
- Quest UI appears when mother gives quest; shows ✓ Complete! on completion
- Dialogue box advances with E; disappears after last line
- Door transitions: fade-out → new room → fade-in, player at correct spawn
- Mother dialogue flow: not-started → active (no item) → complete (has item)
- Orb follows player with hover oscillation
- Settings save/load between sessions (user://settings.cfg)
- No camera swoop on room entry
