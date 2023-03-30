using Godot;
using System;

public partial class SoulIconMenu : SubTypeMenu
{
	public override void _Ready() {
		base._Ready();

		UpdateItems();

		customTextureCallback = new Callable(this, "SetCustomSoulIcon");
	}

	public override void OnItemSelected(long index) {
		base.OnItemSelected(index);

		if (index == customId) {
			return;
		}

		var selectedIcon = cardTypes[(int) index] as SoulIcon;

		Card.instance.SetSoulIcon(selectedIcon);
	}

	public void SetCustomSoulIcon(Texture2D texture) {
		Card.instance.SetCustomSoulIcon(texture);
	}
}
