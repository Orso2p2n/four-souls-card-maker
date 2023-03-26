using Godot;
using Godot.Collections;
using System;

public partial class ForegroundMenu : SubTypeMenu
{
	public override void _Ready() {
		base._Ready();

		customTextureCallback = new Callable(this, "SetCustomForeground");
	}

	public override void OnItemSelected(long index) {
		base.OnItemSelected(index);

		if (index == customId) {
			return;
		}

		var selectedCardType = cardTypes[(int) index] as CardForeground;

		Card.instance.SetCardForeground(selectedCardType);
	}

	public void SetCustomForeground(Texture2D texture) {
		Card.instance.SetCustomCardForeground(texture);
	}
}
