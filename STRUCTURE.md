# The House

## Dimension: 2D (side-scrolling)

## Input Actions

| Action | Keys |
|--------|------|
| move_left | A, Left Arrow |
| move_right | D, Right Arrow |
| run | Left Shift |
| interact | E |
| inventory_next | Mouse Wheel Down |
| inventory_prev | Mouse Wheel Up |
| slot_1 … slot_6 | Keys 1–6 |

## Autoloads (persistent singletons)

| Name | Script | Role |
|------|--------|------|
| SettingsManager | SettingsManager.cs | Volume, fullscreen, sensitivity — persists via ConfigFile |
| GameManager | GameManager.cs | Global state: TargetSpawnPoint, NearestInteractable, interact prompt UI |
| InventoryManager | InventoryManager.cs | 6-slot inventory data + hotbar CanvasLayer |
| QuestManager | QuestManager.cs | Quest state dictionary + quest tracker CanvasLayer |
| DialogueManager | DialogueManager.cs | Dialogue queue + dialogue box CanvasLayer |
| SceneTransitionManager | SceneTransitionManager.cs | Fade overlay + ChangeSceneToFile wrapper |

## Scenes

### main_menu.tscn
- **Root:** Control (full-rect)
- **Script:** MainMenuUI.cs
- **UI:** title label, Play/Settings/Quit buttons, settings panel (hidden by default)

### hallway.tscn
- **Root:** Node2D
- **Script:** HallwayScene.cs (extends RoomBase)
- **Runtime children:** player.tscn, orb.tscn, item_pickups, door areas, ColorRects

### kitchen.tscn
- **Root:** Node2D
- **Script:** KitchenScene.cs (extends RoomBase)
- **Runtime children:** player.tscn, orb.tscn, MotherNPCController, door areas

### bedroom.tscn / living_room.tscn
- **Root:** Node2D
- **Script:** BedroomScene.cs / LivingRoomScene.cs (extends RoomBase)

### player.tscn
- **Root:** CharacterBody2D — PlayerController.cs
- **Children:** CollisionShape2D (Capsule), Sprite2D (hero_spritesheet.png), Camera2D, InteractionArea (Area2D + CircleShape)

### orb.tscn
- **Root:** Node2D — OrbFollower.cs
- **Children:** (PointLight2D created at runtime in _Ready)

## Scripts

### PlayerController (CharacterBody2D)
- Signals: none
- Group: "player"
- Reads: move_left/right, run, interact, slot_1…slot_6, inventory_next/prev
- Calls: InventoryManager.SelectSlot/Next/Prev, GameManager.NearestInteractable.Interact()

### RoomBase (Node2D)
- Instantiates player.tscn and orb.tscn in _Ready()
- Positions player at GameManager.TargetSpawnPoint (from SpawnPoints dict)
- Provides: AddDoor(), AddItem(), AddMother(), AddColorRect()

### InteractableBase (Area2D) — abstract
- BodyEntered → GameManager.SetNearestInteractable(this)
- BodyExited  → GameManager.SetNearestInteractable(null)
- abstract Interact(CharacterBody2D)

### DoorInteraction : InteractableBase
- Interact() → SceneTransitionManager.GoToScene(TargetScene, TargetSpawnPoint)

### ItemPickup : InteractableBase
- Interact() → InventoryManager.AddItem(Item) → QueueFree()
- _Draw() → colored circle

### MotherNPCController : InteractableBase
- State machine: NotStarted → Active → Complete
- Active + hasApple → CompleteQuestDialogue() → InventoryManager.RemoveItem + QuestManager.CompleteQuest
- Draws mother sprite or humanoid placeholder

### OrbFollower (Node2D)
- _Process(): lerp toward (player.GlobalPosition + Offset + hover oscillation)
- _Draw(): three concentric circles (glow, mid, core)

### InventoryManager (Node autoload)
- AddItem / HasItem / RemoveItem / SelectSlot / SelectNext / SelectPrev
- Builds hotbar CanvasLayer (layer=10) in _Ready()

### QuestManager (Node autoload)
- StartQuest / CompleteQuest / IsQuestActive / IsQuestComplete
- Builds quest panel CanvasLayer (layer=10) in _Ready()

### DialogueManager (Node autoload)
- StartDialogue(List<DialogueLine>, Action? onComplete)
- _Input(): E or left-click advances line
- Builds dialogue box CanvasLayer (layer=15) in _Ready()

### SceneTransitionManager (Node autoload)
- GoToScene(scenePath, spawnPoint): fade-out → ChangeSceneToFile → fade-in
- Builds fade overlay CanvasLayer (layer=100) in _Ready()

## Signal Map

- InteractableBase.BodyEntered → GameManager.SetNearestInteractable(this)
- InteractableBase.BodyExited  → GameManager.SetNearestInteractable(null)
- MainMenuUI: Play button.Pressed → SceneTransitionManager.GoToScene(hallway)
- MainMenuUI: Quit button.Pressed → GetTree().Quit()
- MotherNPCController: quest given → QuestManager.StartQuest()
- MotherNPCController: quest complete → QuestManager.CompleteQuest() + InventoryManager.RemoveItem()

## Build Order

1. `dotnet build`
2. `godot --headless --import`
3. `scenes/BuildPlayer.cs` → `scenes/player.tscn`
4. `scenes/BuildOrb.cs` → `scenes/orb.tscn`
5. `scenes/BuildHallway.cs` → `scenes/hallway.tscn`
6. `scenes/BuildKitchen.cs` → `scenes/kitchen.tscn`
7. `scenes/BuildBedroom.cs` → `scenes/bedroom.tscn`
8. `scenes/BuildLivingRoom.cs` → `scenes/living_room.tscn`
9. `scenes/BuildMainMenu.cs` → `scenes/main_menu.tscn`

## Asset Hints

- Hero character sprite: 1274×880 RGBA, 3-view design sheet (front/side/back), ~424px per view
- Mother NPC sprite: 2480×3508 RGBA (white BG removed), full-body portrait
- All item/orb visuals: procedural (no texture files)
- All room backgrounds: procedural ColorRect nodes
