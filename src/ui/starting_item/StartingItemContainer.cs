using Godot;
using System;

public partial class StartingItemContainer : Control
{
	[Export] StartingItemCheckBox checkBox;

	public void SetVisible(bool visible) {
		Visible = visible;

		if (checkBox.ButtonPressed) {
			Card.instance.SetStartingItemVisible(visible);
		}
	}
}
