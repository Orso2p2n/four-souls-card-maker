using Godot;
using Godot.Collections;
using System;

public partial class BorderMenu : SubTypeMenu
{
	public override void OnItemSelected(long index) {
		base.OnItemSelected(index);

		if (index == customId) {
			return;
		}

		var selectedCardType = cardTypes[(int) index] as CardBorder;

		Card.instance.SetCardBorder(selectedCardType);
	}
}
