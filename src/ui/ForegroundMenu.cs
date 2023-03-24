using Godot;
using Godot.Collections;
using System;

public partial class ForegroundMenu : SubTypeMenu
{
	public override void OnItemSelected(long index) {
		var selectedCardType = cardTypes[(int) index] as CardForeground;

		Card.instance.SetCardForeground(selectedCardType);
	}
}
