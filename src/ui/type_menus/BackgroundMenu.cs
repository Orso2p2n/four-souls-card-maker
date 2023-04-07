using Godot;
using Godot.Collections;
using System;

public partial class BackgroundMenu : SubTypeMenu
{
	public override void _Ready() {
		base._Ready();

		customTextureCallback = new Callable(this, "SetCustomBackground");
	}

	public override void OnItemSelected(long index) {
		base.OnItemSelected(index);

		if (index == customId) {
			return;
		}

		var selectedCardType = cardTypes[(int) index] as CardBackground;

		Card.instance.SetCardBackground(selectedCardType);
	}

	public void SetCustomBackground(string path, Texture2D texture) {
		customTexturePath = path;
		Card.instance.SetCustomCardBackground(texture);
	}
}
