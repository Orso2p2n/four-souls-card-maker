using Godot;
using System;

public partial class DifficultyIconMenu : SubTypeMenu
{
	public override void _Ready() {
		base._Ready();

		UpdateItems();

		customTextureCallback = new Callable(this, "SetCustomDifficultyIcon");
	}

	public override void OnItemSelected(long index) {
		base.OnItemSelected(index);
		
		if (index == customId) {
			return;
		}

		var selectedIcon = cardTypes[(int) index] as DifficultyIcon;

		Card.instance.SetDifficultyIcon(selectedIcon);
	}

	public void SetCustomDifficultyIcon(Texture2D texture) {
		Card.instance.SetCustomDifficultyIcon(texture);
	}
}
