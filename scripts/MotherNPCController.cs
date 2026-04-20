using Godot;
using System.Collections.Generic;

/// Mother NPC — stands in the kitchen, gives the "Help Mother" quest.
/// Inherits InteractableBase (Area2D) so the player can talk to her with E.
public partial class MotherNPCController : InteractableBase
{
    private enum QuestState { NotStarted, Active, Complete }
    private QuestState _state = QuestState.NotStarted;

    private const string QuestId        = "HelpMother";
    private const string RequiredItemId = "apple";

    // Mother sprite
    private Sprite2D _sprite = null!;

    public override void _Ready()
    {
        // Restore persistent state — QuestManager survives scene transitions; this instance doesn't.
        if (QuestManager.Instance.IsQuestComplete(QuestId))
            _state = QuestState.Complete;
        else if (QuestManager.Instance.IsQuestActive(QuestId))
            _state = QuestState.Active;
        // else: NotStarted (default)

        InteractLabel = "Talk";
        base._Ready();
        BuildSprite();
    }

    public override void Interact(CharacterBody2D player)
    {
        if (DialogueManager.Instance.IsShowing) return;

        switch (_state)
        {
            case QuestState.NotStarted:
                GiveQuest();
                break;

            case QuestState.Active:
                if (InventoryManager.Instance.HasItem(RequiredItemId))
                    CompleteQuestDialogue();
                else
                    NoItemYet();
                break;

            case QuestState.Complete:
                ThanksAgain();
                break;
        }
    }

    // ─── Dialogue flows ───────────────────────────────────────────────────────

    private void GiveQuest()
    {
        var lines = new List<DialogueLine>
        {
            new("Mother",  "Oh, there you are. I've been looking for you."),
            new("Mother",  "Could you bring me the apple from the hallway table?"),
            new("Mother",  "I'd get it myself, but my back has been acting up today..."),
            new("Player",  "Sure, I'll get it for you."),
            new("Orb",     "An apple? Simple enough. It should be on that table back in the hallway."),
        };
        DialogueManager.Instance.StartDialogue(lines, onComplete: StartQuest);
    }

    private void StartQuest()
    {
        _state = QuestState.Active;
        var quest = new QuestData(
            QuestId,
            "Help Mother",
            "Bring the apple from the hallway table to Mother.",
            RequiredItemId
        );
        QuestManager.Instance.StartQuest(quest);
    }

    private void NoItemYet()
    {
        var lines = new List<DialogueLine>
        {
            new("Mother",  "Did you find the apple?"),
            new("Player",  "Not yet. I'll keep looking."),
            new("Orb",     "It's on the table in the hallway. Hard to miss, actually."),
        };
        DialogueManager.Instance.StartDialogue(lines);
    }

    private void CompleteQuestDialogue()
    {
        var lines = new List<DialogueLine>
        {
            new("Mother",  "Oh, you found it! Thank you so much."),
            new("Player",  "Happy to help."),
            new("Mother",  "You're a good child. Now go rest — the house has been... strange lately."),
            new("Orb",     "Strange. Yes. That's one word for it."),
        };
        DialogueManager.Instance.StartDialogue(lines, onComplete: FinishQuest);
    }

    private void FinishQuest()
    {
        _state = QuestState.Complete;
        InventoryManager.Instance.RemoveItem(RequiredItemId);
        QuestManager.Instance.CompleteQuest(QuestId);
    }

    private void ThanksAgain()
    {
        var lines = new List<DialogueLine>
        {
            new("Mother", "Thank you again, dear. That apple was just what I needed."),
        };
        DialogueManager.Instance.StartDialogue(lines);
    }

    // ─── Visual ──────────────────────────────────────────────────────────────

    private void BuildSprite()
    {
        _sprite = new Sprite2D();

        var tex = GD.Load<Texture2D>("res://sprites/mother_sprite.png");
        if (tex != null)
        {
            _sprite.Texture = tex;
            _sprite.TextureFilter = CanvasItem.TextureFilterEnum.LinearWithMipmaps;
            // Mother sprite is 2480×3508; scale to ~270px height (1080p)
            float scale = 270f / 3508f;
            _sprite.Scale = new Vector2(scale, scale);
            // Shift up so feet align with origin
            _sprite.Offset = new Vector2(0, -3508f / 2f);
        }
        else
        {
            // Fallback: draw a placeholder rectangle
        }
        AddChild(_sprite);
    }

    public override void _Draw()
    {
        // If sprite failed to load, draw a simple humanoid placeholder
        if (_sprite?.Texture == null)
        {
            DrawRect(new Rect2(-18f, -120f, 36f, 80f), new Color(0.55f, 0.25f, 0.5f)); // body
            DrawCircle(new Vector2(0, -135f), 16f, new Color(0.7f, 0.45f, 0.35f));    // head
        }
    }
}
