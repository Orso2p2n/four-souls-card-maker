using Godot;
using System;

public partial class StartingItemContainer : Control
{
	[Export] StartingItemCheckBox checkBox;
	[Export] StartingItemIntroEdit introEdit;
	[Export] StartingItemNameEdit nameEdit;

	public void SetActive(bool active) {
		checkBox.Disabled = !active;
		introEdit.Editable = active;
		nameEdit.Editable = active;

		if (checkBox.ButtonPressed) {
			Card.instance.SetStartingItemVisible(active);
		}
	}
}
