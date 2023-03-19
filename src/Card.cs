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

	public override void _Ready() {
		instance = this;
	}

	public void SetTitle(string text) {
		titleLabel.Text = "[center]" + text;
	}

	public void SetCardType(CardSubType cardType) {
		bgTexture.Texture                      			= (Texture2D) cardType.bgTexture;
		bgOptBorder.Texture 	= fgBorder.Texture 		= (Texture2D) cardType.border;
		bgOptBot.Texture    	= fgBot.Texture     	= (Texture2D) cardType.bot;
		bgOptTop.Texture    	= fgTop.Texture     	= (Texture2D) cardType.top;
	}
}
