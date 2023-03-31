using Godot;
using Godot.Collections;
using System;

[Tool]
public partial class Card : Control
{
	public static Card instance;

	// Exports
	[ExportGroup("Title")]
	[Export] RichTextLabel titleLabel;

	[ExportGroup("Textures")]
	[Export] TextureRect bgTexture;

	[Export] TextureRect bgOptBorder;
	[Export] TextureRect bgOptBot;
	[Export] TextureRect bgOptTop;

	[Export] TextureRect fgBorder;
	[Export] TextureRect fgBot;
	[Export] TextureRect fgTop;

	[ExportGroup("Description")]
	[Export] BoxContainer descContainer;
	[Export] PackedScene effect;
	[Export] PackedScene lore;
	[Export] PackedScene line;

	[ExportGroup("Stats")]
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
	[Export] MoveableArt art;
	[Export] MoveableArtChild topArt;

	[ExportGroup("Icons")]
	[Export] MoveableArt soulIcon;
	[Export] MoveableArt setIcon;
	[Export] MoveableArt diffIcon;

	[ExportGroup("Credits")]
	[Export] RichTextLabel creditsLabel;

	// Signals
	[Signal] public delegate void SelectedArtChangedEventHandler(MoveableArt art = null);

	// Variables
	public MoveableArt curSelectedArt;

	public override void _Ready() {
		instance = this;
	}

	public void SetTitle(string text) {
		titleLabel.Text = "[center]" + text;
	}

	public void SetCredits(string text) {
		creditsLabel.Text = "[right]" + text;
	}

	public void SetCardBackground(CardBackground cardBg) {
		bgTexture.Texture = cardBg.bgTexture;
	}

	public void SetCustomCardBackground(Texture2D texture) {
		bgTexture.Texture = texture;
	}

	public void SetCardForeground(CardForeground cardFg) {
		bgOptBot.Texture = fgBot.Texture = cardFg.bot;
		bgOptTop.Texture = fgTop.Texture = cardFg.top;
	}

	public void SetCustomCardForeground(Texture2D texture) {
		bgOptBot.Texture = fgBot.Texture = texture;
		bgOptTop.Texture = fgTop.Texture = null;
	}

	public void SetCardBorder(CardBorder cardBorder) {
		bgOptBorder.Texture = fgBorder.Texture = cardBorder.border;
	}

	public void SetCustomCardBorder(Texture2D texture) {
		bgOptBorder.Texture = fgBorder.Texture = texture;
	}

	// -- DESCRIPTION --
	public DescEffect AddEffect() {
		var instance = effect.Instantiate();
		instance.ChangeOwner(descContainer);

		return (instance as DescEffect);
	}

	public DescEffect AddLore() {
		var instance = lore.Instantiate();
		instance.ChangeOwner(descContainer);

		return (instance as DescEffect);
	}

	public DescLine AddLine() {
		var instance = line.Instantiate();
		instance.ChangeOwner(descContainer);

		return (instance as DescLine);
	}

	// -- STATS --
	public void setStats(string statsString, double hp, double dice, double atk) {
		monsterStatsContainer.Visible = false;
		characterStatsContainer.Visible = false;

		switch(statsString) {
            case "None": break;

            case "Monster": 
                monsterStatsContainer.Visible = true;
				monsterHp.Text = hp.ToString();
				monsterDice.Text = dice.ToString() + ((dice < 6) ? "+" : "");
				monsterAtk.Text = atk.ToString();
                break;

            case "Character":
                characterStatsContainer.Visible = true;
				characterHp.Text = hp.ToString();
				characterAtk.Text = atk.ToString();
                break;
        }
	}

	// -- ART --
	public void SetArt(Texture2D texture, bool top = false) {
		if (top) {
			topArt.SetTexture(texture);
		}
		else {
			art.SetTexture(texture);
		}
	}

	public void SetArtVisible(bool visible, bool top = false) {
		if (top) {	
			topArt.Visible = visible;
		}
		else {
			art.Visible = visible;
		}
	}

	public void RemoveArt(bool top = false) {
		if (top) {
			topArt.Texture = null;
		}
		else {
			art.Texture = null;
		}
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

	// -- MISC ICONS --
	public void SetSoulIcon(SoulIcon soulIcon) {
		this.soulIcon.SetTexture(soulIcon.texture);
	}

	public void SetCustomSoulIcon(Texture2D texture) {
		soulIcon.SetTexture(texture);
	}

	public void SetSetIcon(SetIcon setIcon) {
		this.setIcon.SetTexture(setIcon.texture);
	}

	public void SetCustomSetIcon(Texture2D texture) {
		setIcon.SetTexture(texture);
	}

	public void SetDifficultyIcon(DifficultyIcon diffIcon) {
		this.diffIcon.SetTexture(diffIcon.texture);
	}

	public void SetCustomDifficultyIcon(Texture2D texture) {
		diffIcon.SetTexture(texture);
	}
}
