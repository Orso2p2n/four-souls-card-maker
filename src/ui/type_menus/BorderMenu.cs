using Godot;
using Godot.Collections;
using System;

public partial class BorderMenu : SubTypeMenu
{
	public override void _Ready() {
		base._Ready();

		customTextureCallback = new Callable(this, "SetCustomBorder");
	}
	
	public override void OnItemSelected(long index) {
		base.OnItemSelected(index);

		if (index == customId) {
			return;
		}

		var selectedCardType = cardTypes[(int) index] as CardBorder;

		Card.instance.SetCardBorder(selectedCardType);
	}

	public void SetCustomBorder(string path, Texture2D texture) {
		customTexturePath = path;
		Card.instance.SetCustomCardBorder(texture);
	}
}
