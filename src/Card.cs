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

	public override void _Ready() {
		instance = this;
	}

	public void SetTitle(string text) {
		titleLabel.Text = "[center]" + text;
	}

	public void SetCardBackground(CardBackground cardBg) {
		bgTexture.Texture = (Texture2D) cardBg.bgTexture;
	}

	public void SetCardForeground(CardForeground cardFg) {
		bgOptBorder.Texture 	= fgBorder.Texture 		= (Texture2D) cardFg.border;
		bgOptBot.Texture    	= fgBot.Texture     	= (Texture2D) cardFg.bot;
		bgOptTop.Texture    	= fgTop.Texture     	= (Texture2D) cardFg.top;
	}

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
}
