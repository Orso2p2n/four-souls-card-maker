using Godot;
using Godot.Collections;
using System;

public partial class BackgroundMenu : SubTypeMenu
{
	public override void OnItemSelected(long index) {
		var selectedCardType = cardTypes[(int) index] as CardBackground;

		Card.instance.SetCardBackground(selectedCardType);
	}
}
