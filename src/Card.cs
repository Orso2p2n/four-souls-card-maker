using Godot;
using Godot.Collections;
using System;

[Tool]
public partial class Card : Control
{
	public static Card instance;

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

	public override void _Ready() {
		instance = this;
	}

	public void SetTitle(string text) {
		titleLabel.Text = "[center]" + text;
	}

	public void SetCardBackground(CardBackground cardBg) {
		bgTexture.Texture = cardBg.bgTexture;
	}

	public void SetCardForeground(CardForeground cardFg) {
		// bgOptBorder.Texture 	= fgBorder.Texture 		= cardFg.border;
		bgOptBot.Texture    	= fgBot.Texture     	= cardFg.bot;
		bgOptTop.Texture    	= fgTop.Texture     	= cardFg.top;
	}

	public void SetCardBorder(CardBorder cardBorder) {
		bgOptBorder.Texture 	= fgBorder.Texture 		= cardBorder.border;
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
}
