using Godot;
using Godot.Collections;
using System;

// [Tool]
public partial class Card : Control
{
	public static Card instance;

	[Signal] public delegate void InputGrabbedEventHandler(InputEvent @event);

	// Exports
	[Export] TextureRect bleedZonesMask;

	[ExportGroup("Title")]
	[Export] AutofitText titleLabel;

	[ExportGroup("Textures")]
	[Export] TextureRect bgTexture;

	[Export] TextureRect bgOptBorder;
	[Export] TextureRect bgOptBot;
	[Export] TextureRect bgOptTop;

	[Export] TextureRect fgBorder;
	[Export] TextureRect fgBot;
	[Export] TextureRect fgTop;

	[ExportGroup("Description")]
	[Export] DescContainer descContainer;
	[Export] PackedScene effect;
	[Export] PackedScene lore;
	[Export] PackedScene line;

	[ExportGroup("Rewards")]
	[Export] TextureRect rewardsTextureRect;
	[Export] AutofitText rewardsLabel;

	[ExportGroup("Stats")]
	[Export] TextureRect customStatsBox;
	[Export] Texture2D monsterStatBoxTexture;
	[Export] Texture2D characterStatBoxTexture;
	[ExportSubgroup("Monster")]
	[Export] Control monsterStatsContainer;
	[Export] Label monsterHp;
	[Export] Label monsterDice;
	[Export] Label monsterAtk;
	[ExportSubgroup("Character")]
	[Export] Control characterStatsContainer;
	[Export] Label characterHp;
	[Export] Label characterAtk;

	[ExportGroup("Art")]
	[Export] public MoveableArt art;
	[Export] public MoveableArtChild topArt;

	[ExportGroup("Icons")]
	[Export] public MoveableArt soulIcon;
	[Export] public MoveableArt setIcon;
	[Export] public MoveableArt diffIcon;

	[ExportGroup("Credits")]
	[Export] RichTextLabel creditsLabel;

	[ExportGroup("StartingItem")]
	[Export] Vector2 descOffsetWhenShown;
	[Export] TextureRect startingItemSeparator;
	[Export] RichTextLabel startingItemIntro;
	[Export] RichTextLabel startingItemName;

	[ExportGroup("Layers")]
	[Export] Control BGLayer;
	[Export] Control FGLayer;

	// Signals
	[Signal] public delegate void SelectedArtChangedEventHandler(MoveableArt art = null);

	// Variables
	public MoveableArt curSelectedArt;
	public Array<MoveableArt> moveableArts;

	private SubViewport cardViewport;

    public override void _EnterTree() {
        instance = this;
		descContainer.card = this;
    }

    public override void _Ready() {
		moveableArts = new Array<MoveableArt>(){art, soulIcon, setIcon, diffIcon};

		cardViewport = GetParent() as SubViewport;
	}

	public void PauseRender() {
		if (cardViewport == null) {
			return;
		}

		cardViewport.RenderTargetUpdateMode = SubViewport.UpdateMode.Disabled;
	}

	public void ResumeRender() {
		if (cardViewport == null) {
			return;
		}

		cardViewport.RenderTargetUpdateMode = SubViewport.UpdateMode.Always;
	}

	public void OnNeedSaveAction() {
		SaveManager.instance.OnNeedSaveAction();
    }

	public void ShowBleedZones() {
		bleedZonesMask.ClipChildren = CanvasItem.ClipChildrenMode.Disabled;
	}

	public void HideBleedZones() {
		bleedZonesMask.ClipChildren = CanvasItem.ClipChildrenMode.Only;
	}

	public void SetTitle(string text) {
		titleLabel.SetText("[center]" + text);
		OnNeedSaveAction();
	}

	public void SetCredits(string text) {
		creditsLabel.Text = "[right]" + text;
		OnNeedSaveAction();
	}

	public void SetCardBackground(CardBackground cardBg) {
		bgTexture.Texture = cardBg.bgTexture;
		OnNeedSaveAction();
	}

	public void SetCustomCardBackground(Texture2D texture) {
		bgTexture.Texture = texture;
		OnNeedSaveAction();
	}

	public void SetCardForeground(CardForeground cardFg) {
		bgOptBot.Texture = fgBot.Texture = cardFg.bot;
		bgOptTop.Texture = fgTop.Texture = cardFg.top;
		OnNeedSaveAction();
	}

	public void SetCustomCardForeground(Texture2D texture) {
		bgOptBot.Texture = fgBot.Texture = texture;
		bgOptTop.Texture = fgTop.Texture = null;
		OnNeedSaveAction();
	}

	public void SetCardBorder(CardBorder cardBorder) {
		bgOptBorder.Texture = fgBorder.Texture = cardBorder.border;
		OnNeedSaveAction();
	}

	public void SetCustomCardBorder(Texture2D texture) {
		bgOptBorder.Texture = fgBorder.Texture = texture;
		OnNeedSaveAction();
	}

	// -- DESCRIPTION --
	public void SetDescOffsets(float top = -1, float bot = -1) {
		descContainer.SetOffsets(top, bot);
		OnNeedSaveAction();
	}

	public DescEffect AddText(bool isLore = false) {
		var instance = isLore ? lore.Instantiate() : effect.Instantiate();
		var descEffect = instance as DescEffect;

		descContainer.AddText(descEffect);

		OnNeedSaveAction();

		return (instance as DescEffect);
	}

	public DescLine AddLine() {
		var instance = line.Instantiate();
		var descLine = instance as DescLine;

		descContainer.AddLine(descLine);

		OnNeedSaveAction();

		return descLine;
	}

	// -- REWARDS --
	public void SetRewardsEnabled(bool enabled, bool builtin = false) {
		if (!enabled) {
			rewardsTextureRect.Visible = false;
			rewardsLabel.Visible = false;
			SetDescOffsets(bot: 0);
			return;
		}

		rewardsTextureRect.Visible = !builtin;
		rewardsLabel.Visible = true;
		SetDescOffsets(bot: 89);
	}

	public void SetRewards(string text) {
		rewardsLabel.SetText("[center]" + text);
		OnNeedSaveAction();
	}

	// -- STATS --
	public void setStats(string statsString, double hp, double dice, double atk, bool hasCustomStats = false) {
		monsterStatsContainer.Visible = false;
		characterStatsContainer.Visible = false;

		switch(statsString) {
            case "None": 
				SetDescOffsets(top: 0);
				break;

            case "Monster":
				customStatsBox.Texture = monsterStatBoxTexture;
                monsterStatsContainer.Visible = true;
				monsterHp.Text = hp.ToString();
				monsterDice.Text = dice.ToString() + ((dice > 0 && dice < 6) ? "+" : " ");
				monsterAtk.Text = atk.ToString();
				SetDescOffsets(top: 54);
                break;

            case "Character":
				customStatsBox.Texture = characterStatBoxTexture;
                characterStatsContainer.Visible = true;
				characterHp.Text = hp.ToString();
				characterAtk.Text = atk.ToString();
				SetDescOffsets(top: 54);
                break;
        }

		customStatsBox.Visible = statsString != "None" && hasCustomStats;

		OnNeedSaveAction();
	}

	// -- ART --
	public void SetArt(Texture2D texture, bool top = false, string path = "") {
		if (top) {
			topArt.SetTexture(texture, path);
		}
		else {
			art.SetTexture(texture, path);
		}

		OnNeedSaveAction();
	}

	public void SetArtVisible(bool visible, bool top = false) {
		if (top) {	
			topArt.Visible = visible;
		}
		else {
			art.Visible = visible;
		}

		OnNeedSaveAction();
	}

	public void RemoveArt(bool top = false) {
		if (top) {
			topArt.Texture = null;
		}
		else {
			art.Texture = null;
		}

		OnNeedSaveAction();
	}

	// -- MOVEABLE ARTS --
	public void OnSelectedArt(MoveableArt art) {
		curSelectedArt = art;
		EmitSignal(SignalName.SelectedArtChanged, art);
	}

	public void OnDeselectedArt(MoveableArt art) {
		if (curSelectedArt == art) {
			curSelectedArt = null;
			EmitSignal(SignalName.SelectedArtChanged, new MoveableArt());
		}
	}

	public void AddMoveableArt(MoveableArt art) {
		art.ChangeOwner(FGLayer);
		moveableArts.Add(art);

		OnNeedSaveAction();
	}

	public void RemoveMoveableArt(MoveableArt art) {
		moveableArts.Remove(art);

		OnNeedSaveAction();
	}

	public void DeselectAllMoveableArts() {
		foreach (var art in moveableArts) {
			art.Deselect();
		}
	}

	// -- MISC ICONS --
	public void SetSoulIcon(SoulIcon soulIcon) {
		this.soulIcon.SetTexture(soulIcon.texture);

		OnNeedSaveAction();
	}

	public void SetCustomSoulIcon(Texture2D texture) {
		soulIcon.SetTexture(texture);

		OnNeedSaveAction();
	}

	public void SetSetIcon(SetIcon setIcon) {
		this.setIcon.SetTexture(setIcon.texture);

		OnNeedSaveAction();
	}

	public void SetCustomSetIcon(Texture2D texture) {
		setIcon.SetTexture(texture);

		OnNeedSaveAction();
	}

	public void SetDifficultyIcon(DifficultyIcon diffIcon) {
		this.diffIcon.SetTexture(diffIcon.texture);

		OnNeedSaveAction();
	}

	public void SetCustomDifficultyIcon(Texture2D texture) {
		diffIcon.SetTexture(texture);

		OnNeedSaveAction();
	}

	// -- STARTING ITEM --
	public void SetStartingItemIntro(string text) {
		startingItemIntro.Text = "[center]" + text;
	}

	public void SetStartingItemName(string text) {
		startingItemName.Text = "[center]" + text;
	}

	public void SetStartingItemVisible(bool visible) {
		if (visible) {
			SetDescOffsets(bot: descOffsetWhenShown.Y);
		}
		else {
			SetDescOffsets(bot: 0);
		}

		startingItemSeparator.Visible = startingItemName.Visible = startingItemIntro.Visible = visible;

		OnNeedSaveAction();
	}

	void OnInputGrabberInputEvent(Node viewport, InputEvent @event, long shapeIdx) {
		EmitSignal(SignalName.InputGrabbed, @event);
	}
}
